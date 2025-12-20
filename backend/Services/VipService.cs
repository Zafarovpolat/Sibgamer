using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace backend.Services;

    public interface IVipService
    {
        Task<bool> TestConnectionAsync(int? serverId = null);
        Task<bool> AddVipAsync(string steamId, string name, string groupName, int durationSeconds);
        Task<bool> UpdateVipExpiryAsync(string steamId, int durationSeconds);
        Task<bool> RemoveVipAsync(string steamId);
        Task<Dictionary<string, object>?> GetVipBySteamIdAsync(string steamId);
    }public class VipService : IVipService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VipService> _logger;

    public VipService(ApplicationDbContext context, ILogger<VipService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private async Task<string?> GetConnectionStringAsync(int? serverId = null)
    {
        VipSettings? settings;

        if (serverId.HasValue)
        {
            settings = await _context.VipSettings
                .FirstOrDefaultAsync(s => s.ServerId == serverId && s.IsConfigured);
        }
        else
        {
            settings = await _context.VipSettings
                .Where(s => s.IsConfigured)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();
        }

        if (settings == null)
        {
            _logger.LogWarning("VIP settings not configured" + (serverId.HasValue ? $" for server {serverId}" : ""));
            return null;
        }

        return $"Server={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.Username};Password={settings.Password};Connect Timeout=30;";
    }

    public async Task<bool> TestConnectionAsync(int? serverId = null)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync(serverId);
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM vip_users LIMIT 1";
            await cmd.ExecuteScalarAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to VIP database" + (serverId.HasValue ? $" for server {serverId}" : ""));
            return false;
        }
    }

    public async Task<bool> AddVipAsync(string steamId, string name, string groupName, int durationSeconds)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
            {
                _logger.LogError("VIP connection string is not configured");
                return false;
            }

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var accountId = SteamIdToAccountId(steamId);
            
            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT account_id FROM vip_users WHERE account_id = @steamId AND (expires = 0 OR expires > UNIX_TIMESTAMP())";
            checkCmd.Parameters.AddWithValue("@steamId", accountId);

            var existingVip = await checkCmd.ExecuteScalarAsync();
            if (existingVip != null)
            {
                _logger.LogInformation("VIP with Steam ID {SteamId} already exists, updating expiry", steamId);
                return await UpdateVipExpiryAsync(steamId, durationSeconds);
            }

            using var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO vip_users (account_id, name, lastvisit, `group`, expires)
                VALUES (@accountId, @name, UNIX_TIMESTAMP(), @groupName, @expires)
                ON DUPLICATE KEY UPDATE 
                    name = VALUES(name), 
                    lastvisit = VALUES(lastvisit), 
                    `group` = VALUES(`group`), 
                    expires = VALUES(expires)";

            insertCmd.Parameters.AddWithValue("@accountId", accountId);
            insertCmd.Parameters.AddWithValue("@name", name);
            insertCmd.Parameters.AddWithValue("@groupName", groupName);

            var expiresTimestamp = durationSeconds == 0
                ? 0
                : DateTimeOffset.Now.ToUnixTimeSeconds() + durationSeconds;
            insertCmd.Parameters.AddWithValue("@expires", expiresTimestamp);

            await insertCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Successfully added VIP {SteamId} to VIP database with group {GroupName}", steamId, groupName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding VIP to VIP database");
            return false;
        }
    }

    public async Task<bool> UpdateVipExpiryAsync(string steamId, int durationSeconds)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var accountId = SteamIdToAccountId(steamId);

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT expires FROM vip_users WHERE account_id = @accountId";
            selectCmd.Parameters.AddWithValue("@accountId", accountId);

            var currentEnds = await selectCmd.ExecuteScalarAsync();
            if (currentEnds == null)
                return false;

            var currentEndsTimestamp = Convert.ToInt64(currentEnds);
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            var baseTimestamp = currentEndsTimestamp > now ? currentEndsTimestamp : now;
            var newEndsTimestamp = durationSeconds == 0 ? 0 : baseTimestamp + durationSeconds;

            using var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = "UPDATE vip_users SET expires = @expires WHERE account_id = @accountId";
            updateCmd.Parameters.AddWithValue("@expires", newEndsTimestamp);
            updateCmd.Parameters.AddWithValue("@accountId", accountId);

            var rowsAffected = await updateCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Updated VIP {SteamId} expiry to {NewEnds}", steamId, newEndsTimestamp);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating VIP expiry in VIP database");
            return false;
        }
    }

    public async Task<bool> RemoveVipAsync(string steamId)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var accountId = SteamIdToAccountId(steamId);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM vip_users WHERE account_id = @accountId";
            cmd.Parameters.AddWithValue("@accountId", accountId);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Removed VIP {SteamId} from VIP database", steamId);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing VIP from VIP database");
            return false;
        }
    }

    public async Task<Dictionary<string, object>?> GetVipBySteamIdAsync(string steamId)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return null;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var accountId = SteamIdToAccountId(steamId);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT account_id, name, lastvisit, `group`, expires
                FROM vip_users
                WHERE account_id = @accountId
                AND (expires = 0 OR expires > UNIX_TIMESTAMP())
                LIMIT 1";
            cmd.Parameters.AddWithValue("@accountId", accountId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new Dictionary<string, object>
                {
                    ["account_id"] = reader.GetInt32(0),
                    ["name"] = reader.GetString(1),
                    ["lastvisit"] = reader.GetInt64(2),
                    ["group"] = reader.GetString(3),
                    ["expires"] = reader.GetInt64(4)
                };
                return result;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting VIP from VIP database");
            return null;
        }
    }

    private static int SteamIdToAccountId(string steamId)
    {
        var parts = steamId.Split(':');
        if (parts.Length >= 3)
        {
            return int.Parse(parts[2]) * 2 + int.Parse(parts[1]);
        }
        throw new ArgumentException("Invalid Steam ID format");
    }
}
