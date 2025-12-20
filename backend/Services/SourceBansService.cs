using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace backend.Services;

public interface ISourceBansService
{
    Task<bool> TestConnectionAsync();
    Task<int?> AddAdminAsync(string steamId, string name, string? flags, string? groupName, int immunity, int durationSeconds, string? password = null);
    Task<bool> UpdateAdminExpiryAsync(int adminId, int durationSeconds);
    Task<bool> UpdateAdminPasswordAsync(int adminId, string password);
    Task<bool> RemoveAdminAsync(int adminId);
    Task<Dictionary<string, object>?> GetAdminBySteamIdAsync(string steamId);

}

public class SourceBansService : ISourceBansService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SourceBansService> _logger;
    private readonly IRconService _rconService;

    public SourceBansService(
        ApplicationDbContext context,
        ILogger<SourceBansService> logger,
        IRconService rconService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rconService = rconService ?? throw new ArgumentNullException(nameof(rconService));
    }

    private async Task<string?> GetConnectionStringAsync()
    {
        var settings = await _context.SourceBansSettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            _logger.LogWarning("SourceBans settings not configured");
            return null;
        }

        return $"Server={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.Username};Password={settings.Password};";
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM sb_admins LIMIT 1";
            await cmd.ExecuteScalarAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to SourceBans database");
            return false;
        }
    }

    public async Task<int?> AddAdminAsync(string steamId, string name, string? flags, string? groupName, int immunity, int durationSeconds, string? password = null)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
            {
                _logger.LogError("SourceBans connection string is not configured");
                return null;
            }

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var settings = await _context.SourceBansSettings
                .Include(s => s.Server)
                .Where(s => s.IsConfigured)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            if (settings == null || settings.Server == null)
            {
                _logger.LogError("SourceBans settings or server info not found");
                return null;
            }

            var sourceBansServerId = await GetSourceBansServerIdAsync(connection, settings.Server.IpAddress, settings.Server.Port);
            if (sourceBansServerId == null)
            {
                _logger.LogError("Could not find SourceBans server with IP {Ip} and Port {Port}", settings.Server.IpAddress, settings.Server.Port);
                return null;
            }

            bool hasExpiredColumn;
            using (var colChk = connection.CreateCommand())
            {
                colChk.CommandText = "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins' AND column_name = 'expired'";
                var colRes = await colChk.ExecuteScalarAsync();
                hasExpiredColumn = Convert.ToInt32(colRes) > 0;
            }

            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = hasExpiredColumn
                ? "SELECT aid FROM sb_admins WHERE authid = @steamId AND (expired = 0 OR expired > UNIX_TIMESTAMP())"
                : "SELECT aid FROM sb_admins WHERE authid = @steamId";
            checkCmd.Parameters.AddWithValue("@steamId", steamId);
            
            var existingAdminId = await checkCmd.ExecuteScalarAsync();
            if (existingAdminId != null)
            {
                _logger.LogInformation("Admin with Steam ID {SteamId} already exists, updating expiry", steamId);
                var existingAdminIdInt = Convert.ToInt32(existingAdminId);
                await UpdateAdminExpiryAsync(existingAdminIdInt, durationSeconds);
                return existingAdminIdInt;
            }

            using var insertAdminCmd = connection.CreateCommand();

            var adminCols = new List<string> { "user", "authid", "password", "gid", "email", "validate", "extraflags", "immunity", "lastvisit" };

            if (hasExpiredColumn)
            {
                adminCols.Add("expired");
            }

            bool hasSrvGroup = false, hasSrvFlags = false, hasSrvPassword = false;
            using (var colChk2 = connection.CreateCommand())
            {
                colChk2.CommandText = "SELECT column_name FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins' AND column_name IN ('srv_group','srv_flags','srv_password')";
                using var reader = await colChk2.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var c = reader.GetString(0);
                    if (c == "srv_group") hasSrvGroup = true;
                    if (c == "srv_flags") hasSrvFlags = true;
                    if (c == "srv_password") hasSrvPassword = true;
                }
            }

            if (hasSrvGroup) adminCols.Add("srv_group");
            if (hasSrvFlags) adminCols.Add("srv_flags");
            if (hasSrvPassword) adminCols.Add("srv_password");

            var columnList = string.Join(", ", adminCols);
            var paramList = string.Join(", ", adminCols.Select(c => "@" + c));

            insertAdminCmd.CommandText = $@"INSERT INTO sb_admins ({columnList}) VALUES ({paramList})";
            
            var baseUser = (name ?? "user").Replace('\n', ' ').Replace('\r', ' ').Trim();
            if (string.IsNullOrEmpty(baseUser)) baseUser = "user";
            if (baseUser.Length > 60) baseUser = baseUser.Substring(0, 60);

            string chosenUser = baseUser;
            int attempt = 0;
            while (true)
            {
                using var checkUserCmd = connection.CreateCommand();
                checkUserCmd.CommandText = "SELECT COUNT(*) FROM sb_admins WHERE `user` = @userVal";
                checkUserCmd.Parameters.AddWithValue("@userVal", chosenUser);
                var res = await checkUserCmd.ExecuteScalarAsync();
                var exists = Convert.ToInt32(res) > 0;
                if (!exists)
                    break;

                attempt++;
                chosenUser = baseUser + "_" + new Random().Next(1000, 9999).ToString();
                if (attempt > 10)
                {
                    chosenUser = baseUser + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();
                    break;
                }
            }

            insertAdminCmd.Parameters.AddWithValue("@user", chosenUser);
            insertAdminCmd.Parameters.AddWithValue("@authid", steamId ?? string.Empty);
            insertAdminCmd.Parameters.AddWithValue("@password", password ?? string.Empty);
            insertAdminCmd.Parameters.AddWithValue("@gid", 0);
            insertAdminCmd.Parameters.AddWithValue("@email", string.Empty);
            insertAdminCmd.Parameters.AddWithValue("@validate", 0);
            insertAdminCmd.Parameters.AddWithValue("@extraflags", string.Empty);
            insertAdminCmd.Parameters.AddWithValue("@immunity", immunity);
            insertAdminCmd.Parameters.AddWithValue("@lastvisit", DateTimeOffset.Now.ToUnixTimeSeconds());

            if (hasExpiredColumn)
            {
                var expiredTimestamp = durationSeconds == 0 ? 0 : DateTimeOffset.Now.ToUnixTimeSeconds() + durationSeconds;
                insertAdminCmd.Parameters.AddWithValue("@expired", expiredTimestamp);
            }

            if (hasSrvGroup)
            {
                insertAdminCmd.Parameters.AddWithValue("@srv_group", groupName ?? "");
            }
            if (hasSrvFlags)
            {
                insertAdminCmd.Parameters.AddWithValue("@srv_flags", flags ?? "");
            }
            if (hasSrvPassword)
            {
                insertAdminCmd.Parameters.AddWithValue("@srv_password", password ?? "");
            }

            await insertAdminCmd.ExecuteNonQueryAsync();

            insertAdminCmd.CommandText = "SELECT LAST_INSERT_ID()";
            var newAdminId = await insertAdminCmd.ExecuteScalarAsync();
            var adminId = Convert.ToInt32(newAdminId);

            using var insertServerGroupCmd = connection.CreateCommand();

            var groupCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var colChk3 = connection.CreateCommand())
            {
                colChk3.CommandText = "SELECT column_name FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins_servers_groups'";
                using var reader = await colChk3.ExecuteReaderAsync();
                while (await reader.ReadAsync()) groupCols.Add(reader.GetString(0));
            }

            if (groupCols.Contains("srv_group") || groupCols.Contains("srv_flags"))
            {
                insertServerGroupCmd.CommandText = @"
                    INSERT INTO sb_admins_servers_groups (admin_id, server_id, srv_group, srv_flags, srv_password, immunity)
                    VALUES (@adminId, @serverId, @groupName, @flags, @password, @immunity)";
                insertServerGroupCmd.Parameters.AddWithValue("@adminId", adminId);
                insertServerGroupCmd.Parameters.AddWithValue("@serverId", sourceBansServerId);
                insertServerGroupCmd.Parameters.AddWithValue("@groupName", groupName ?? "");
                insertServerGroupCmd.Parameters.AddWithValue("@flags", flags ?? "");
                insertServerGroupCmd.Parameters.AddWithValue("@password", password ?? "");
                insertServerGroupCmd.Parameters.AddWithValue("@immunity", immunity);
            }
            else if (groupCols.Contains("admin_id") && groupCols.Contains("server_id") && groupCols.Contains("group_id") && groupCols.Contains("srv_group_id"))
            {
                insertServerGroupCmd.CommandText = @"
                    INSERT INTO sb_admins_servers_groups (admin_id, group_id, srv_group_id, server_id)
                    VALUES (@adminId, @groupId, @srvGroupId, @serverId)";
                insertServerGroupCmd.Parameters.AddWithValue("@adminId", adminId);
                insertServerGroupCmd.Parameters.AddWithValue("@groupId", 0); 
                insertServerGroupCmd.Parameters.AddWithValue("@srvGroupId", -1); 
                insertServerGroupCmd.Parameters.AddWithValue("@serverId", sourceBansServerId);
            }
            else
            {
                insertServerGroupCmd.CommandText = @"INSERT INTO sb_admins_servers_groups (admin_id, server_id) VALUES (@adminId, @serverId)";
                insertServerGroupCmd.Parameters.AddWithValue("@adminId", adminId);
                insertServerGroupCmd.Parameters.AddWithValue("@serverId", sourceBansServerId);
            }
            
            await insertServerGroupCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Successfully added admin {SteamId} to SourceBans with ID {AdminId}", steamId, adminId);

            try
            {
                var server = settings.Server;
                if (server != null && !string.IsNullOrEmpty(server.RconPassword))
                {
                    await _rconService.ExecuteCommandsAsync(server.IpAddress, server.Port, server.RconPassword, "sm_reloadadmins", "sm_rehash");
                    _logger.LogInformation("Sent RCON reload commands to {Ip}:{Port}", server.IpAddress, server.Port);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to execute RCON commands after adding admin");
            }
            return adminId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding admin to SourceBans");
            return null;
        }
    }

    public async Task<bool> UpdateAdminPasswordAsync(int adminId, string password)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var settings = await _context.SourceBansSettings
                .Include(s => s.Server)
                .Where(s => s.IsConfigured)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            if (settings == null || settings.Server == null)
            {
                _logger.LogError("SourceBans settings or server info not found");
                return false;
            }

            var sourceBansServerId = await GetSourceBansServerIdAsync(connection, settings.Server.IpAddress, settings.Server.Port);
            if (sourceBansServerId == null)
            {
                _logger.LogError("Could not find SourceBans server with IP {Ip} and Port {Port}", settings.Server.IpAddress, settings.Server.Port);
                return false;
            }

            bool hasSrvPasswordInServerGroups = false;
            bool hasSrvPasswordInAdmins = false;

            using (var colChk = connection.CreateCommand())
            {
                colChk.CommandText = "SELECT column_name FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins_servers_groups' AND column_name = 'srv_password'";
                var exists = await colChk.ExecuteScalarAsync();
                hasSrvPasswordInServerGroups = exists != null;
            }

            if (!hasSrvPasswordInServerGroups)
            {
                using var colChk2 = connection.CreateCommand();
                colChk2.CommandText = "SELECT column_name FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins' AND column_name = 'srv_password'";
                var exists2 = await colChk2.ExecuteScalarAsync();
                hasSrvPasswordInAdmins = exists2 != null;
            }

            int rowsAffected = 0;
            if (hasSrvPasswordInServerGroups)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE sb_admins_servers_groups SET srv_password = @password WHERE admin_id = @adminId AND server_id = @serverId";
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@adminId", adminId);
                cmd.Parameters.AddWithValue("@serverId", sourceBansServerId);

                rowsAffected = Convert.ToInt32(await cmd.ExecuteNonQueryAsync());
            }
            else if (hasSrvPasswordInAdmins)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE sb_admins SET srv_password = @password WHERE aid = @adminId";
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@adminId", adminId);

                rowsAffected = Convert.ToInt32(await cmd.ExecuteNonQueryAsync());
            }
            else
            {
                _logger.LogWarning("SourceBans schema does not contain srv_password column in sb_admins_servers_groups nor sb_admins — cannot update admin password");
                return false;
            }
            
            _logger.LogInformation("Updated password for admin {AdminId}", adminId);

            try
            {
                var reloadSettings = await _context.SourceBansSettings
                    .Include(s => s.Server)
                    .Where(s => s.IsConfigured)
                    .OrderByDescending(s => s.UpdatedAt)
                    .FirstOrDefaultAsync();
                if (reloadSettings?.Server != null && !string.IsNullOrEmpty(reloadSettings.Server.RconPassword))
                {
                    await _rconService.ExecuteCommandsAsync(reloadSettings.Server.IpAddress, reloadSettings.Server.Port, reloadSettings.Server.RconPassword, "sm_reloadadmins", "sm_rehash");
                    _logger.LogInformation("Sent RCON reload commands to {Ip}:{Port} after password update", reloadSettings.Server.IpAddress, reloadSettings.Server.Port);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to execute RCON commands after updating admin password");
            }
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating admin password in SourceBans");
            return false;
        }
    }

    public async Task<bool> UpdateAdminExpiryAsync(int adminId, int durationSeconds)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var expExistsCmd = connection.CreateCommand();
            expExistsCmd.CommandText = "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins' AND column_name = 'expired'";
            var expExistsRes = await expExistsCmd.ExecuteScalarAsync();
            var hasExpiredColumn = Convert.ToInt32(expExistsRes) > 0;

            if (!hasExpiredColumn)
            {
                _logger.LogWarning("SourceBans sb_admins table does not contain 'expired' column -> cannot update expiry");
                return false;
            }

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT expired FROM sb_admins WHERE aid = @adminId";
            selectCmd.Parameters.AddWithValue("@adminId", adminId);
            
            var currentEnds = await selectCmd.ExecuteScalarAsync();
            if (currentEnds == null)
                return false;

            var currentEndsTimestamp = Convert.ToInt64(currentEnds);
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            var baseTimestamp = currentEndsTimestamp > now ? currentEndsTimestamp : now;
            var newEndsTimestamp = durationSeconds == 0 ? 0 : baseTimestamp + durationSeconds;

            using var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = "UPDATE sb_admins SET expired = @expired WHERE aid = @adminId";
            updateCmd.Parameters.AddWithValue("@expired", newEndsTimestamp);
            updateCmd.Parameters.AddWithValue("@adminId", adminId);

            var rowsAffected = await updateCmd.ExecuteNonQueryAsync();
            
            _logger.LogInformation("Updated admin {AdminId} expiry to {NewEnds}", adminId, newEndsTimestamp);

            try
            {
                var settings = await _context.SourceBansSettings
                    .Include(s => s.Server)
                    .Where(s => s.IsConfigured)
                    .OrderByDescending(s => s.UpdatedAt)
                    .FirstOrDefaultAsync();

                if (settings?.Server != null && !string.IsNullOrEmpty(settings.Server.RconPassword))
                {
                    await _rconService.ExecuteCommandsAsync(settings.Server.IpAddress, settings.Server.Port, settings.Server.RconPassword, "sm_reloadadmins", "sm_rehash");
                    _logger.LogInformation("Sent RCON reload commands to {Ip}:{Port} after expiry update", settings.Server.IpAddress, settings.Server.Port);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to execute RCON commands after updating admin expiry");
            }
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating admin expiry in SourceBans");
            return false;
        }
    }

    public async Task<bool> RemoveAdminAsync(int adminId)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return false;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var deleteServerGroupCmd = connection.CreateCommand();
            deleteServerGroupCmd.CommandText = "DELETE FROM sb_admins_servers_groups WHERE admin_id = @adminId";
            deleteServerGroupCmd.Parameters.AddWithValue("@adminId", adminId);
            await deleteServerGroupCmd.ExecuteNonQueryAsync();

            using var deleteAdminCmd = connection.CreateCommand();
            deleteAdminCmd.CommandText = "DELETE FROM sb_admins WHERE aid = @adminId";
            deleteAdminCmd.Parameters.AddWithValue("@adminId", adminId);

            var rowsAffected = await deleteAdminCmd.ExecuteNonQueryAsync();
            
            _logger.LogInformation("Removed admin {AdminId} from SourceBans", adminId);

            try
            {
                var reloadSettings = await _context.SourceBansSettings
                    .Include(s => s.Server)
                    .Where(s => s.IsConfigured)
                    .OrderByDescending(s => s.UpdatedAt)
                    .FirstOrDefaultAsync();

                if (reloadSettings?.Server != null && !string.IsNullOrEmpty(reloadSettings.Server.RconPassword))
                {
                    await _rconService.ExecuteCommandsAsync(reloadSettings.Server.IpAddress, reloadSettings.Server.Port, reloadSettings.Server.RconPassword, "sm_reloadadmins", "sm_rehash");
                    _logger.LogInformation("Sent RCON reload commands to {Ip}:{Port} after admin removal", reloadSettings.Server.IpAddress, reloadSettings.Server.Port);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to execute RCON commands after removing admin");
            }
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing admin from SourceBans");
            return false;
        }
    }

    public async Task<Dictionary<string, object>?> GetAdminBySteamIdAsync(string steamId)
    {
        try
        {
            var connectionString = await GetConnectionStringAsync();
            if (connectionString == null)
                return null;

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            bool hasExpired;
            using (var expChk = connection.CreateCommand())
            {
                expChk.CommandText = "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE table_schema = DATABASE() AND table_name = 'sb_admins' AND column_name = 'expired'";
                var expRes = await expChk.ExecuteScalarAsync();
                hasExpired = Convert.ToInt32(expRes) > 0;
            }

            using var cmd = connection.CreateCommand();
            if (hasExpired)
            {
                cmd.CommandText = @"
                SELECT a.aid, a.user, a.authid, asg.srv_flags, asg.srv_group, a.immunity, a.lastvisit, a.expired
                FROM sb_admins a
                LEFT JOIN sb_admins_servers_groups asg ON a.aid = asg.admin_id
                WHERE a.authid = @steamId 
                AND (a.expired = 0 OR a.expired > UNIX_TIMESTAMP())
                LIMIT 1";
            }
            else
            {
                cmd.CommandText = @"
                SELECT a.aid, a.user, a.authid, asg.srv_flags, asg.srv_group, a.immunity, a.lastvisit, NULL as expired
                FROM sb_admins a
                LEFT JOIN sb_admins_servers_groups asg ON a.aid = asg.admin_id
                WHERE a.authid = @steamId
                LIMIT 1";
            }
            cmd.Parameters.AddWithValue("@steamId", steamId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new Dictionary<string, object>
                {
                    ["aid"] = reader.GetInt32(0),
                    ["user"] = reader.GetString(1),
                    ["authid"] = reader.GetString(2),
                    ["srv_flags"] = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    ["srv_group"] = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    ["immunity"] = reader.GetInt32(5),
                    ["created"] = reader.GetInt64(6),
                    ["expired"] = reader.GetInt64(7)
                };
                return result;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting admin from SourceBans");
            return null;
        }
    }

    private async Task<int?> GetSourceBansServerIdAsync(MySqlConnection connection, string ipAddress, int port)
    {
        try
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT sid FROM sb_servers WHERE ip = @ip AND port = @port LIMIT 1";
            cmd.Parameters.AddWithValue("@ip", ipAddress);
            cmd.Parameters.AddWithValue("@port", port);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SourceBans server ID for {Ip}:{Port}", ipAddress, port);
            return null;
        }
    }
}
