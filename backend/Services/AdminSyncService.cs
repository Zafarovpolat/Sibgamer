using backend.Data;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Services;

public interface IAdminSyncService
{
    Task SyncAllAdminsAsync();
    Task SyncUserAdminsAsync(int userId);
    Task<List<AdminStatusDto>> GetUserAdminStatusesAsync(int userId);
    Task<bool> SyncUserAdminStatusAsync(int userId, string steamId);
}

public class AdminSyncService : IAdminSyncService
{
    private readonly ApplicationDbContext _context;
    private readonly ISourceBansService _sourceBansService;
    private readonly ILogger<AdminSyncService> _logger;

    public AdminSyncService(
        ApplicationDbContext context,
        ISourceBansService sourceBansService,
        ILogger<AdminSyncService> logger)
    {
        _context = context;
        _sourceBansService = sourceBansService;
        _logger = logger;
    }

    public async Task SyncAllAdminsAsync()
    {
        _logger.LogInformation("Starting full admin synchronization");

        try
        {
            var activePrivileges = await _context.UserAdminPrivileges
                .Include(p => p.User)
                .Include(p => p.Server)
                .Where(p => p.IsActive && p.ExpiresAt > DateTimeHelper.GetServerLocalTime())
                .ToListAsync();

            var syncedCount = 0;
            var updatedCount = 0;

            foreach (var privilege in activePrivileges)
            {
                try
                {
                    var isSynced = await SyncUserAdminStatusAsync(privilege.UserId, privilege.SteamId);
                    if (isSynced)
                    {
                        syncedCount++;
                        privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing admin status for user {UserId}, SteamId {SteamId}",
                        privilege.UserId, privilege.SteamId);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Admin synchronization completed. Synced: {SyncedCount}, Updated: {UpdatedCount}",
                syncedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during full admin synchronization");
            throw;
        }
    }

    public async Task SyncUserAdminsAsync(int userId)
    {
        _logger.LogInformation("Starting admin synchronization for user {UserId}", userId);

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.SteamId))
            {
                _logger.LogWarning("User {UserId} not found or has no SteamId", userId);
                return;
            }

            await SyncUserAdminStatusAsync(userId, user.SteamId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing admins for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<AdminStatusDto>> GetUserAdminStatusesAsync(int userId)
    {
        var privileges = await _context.UserAdminPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.IsActive)
            .ThenByDescending(p => p.ExpiresAt)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = new List<AdminStatusDto>();

        foreach (var privilege in privileges)
        {
            var status = new AdminStatusDto
            {
                ServerId = privilege.ServerId,
                ServerName = privilege.Server?.Name ?? "Unknown Server",
                TariffName = privilege.Tariff?.Name ?? "Unknown Tariff",
                Flags = privilege.Flags,
                GroupName = privilege.GroupName,
                Immunity = privilege.Immunity,
                StartsAt = privilege.StartsAt,
                ExpiresAt = privilege.ExpiresAt,
                IsActive = privilege.IsActive && privilege.ExpiresAt > now,
                IsExpired = privilege.ExpiresAt <= now,
                DaysRemaining = privilege.ExpiresAt > now ? (int)(privilege.ExpiresAt - now).TotalDays : 0,
                LastSync = privilege.UpdatedAt
            };

            result.Add(status);
        }

        return result;
    }

    public async Task<bool> SyncUserAdminStatusAsync(int userId, string steamId)
    {
        _logger.LogInformation("Syncing admin status for user {UserId} with SteamId {SteamId}", userId, steamId);

        try
        {
            var sourceBansServers = await _context.SourceBansSettings
                .Where(s => s.IsConfigured)
                .Include(s => s.Server)
                .ToListAsync();

            var hasChanges = false;

            foreach (var sourceBansSetting in sourceBansServers)
            {
                try
                {
                    var adminData = await _sourceBansService.GetAdminBySteamIdAsync(steamId);

                    if (adminData != null)
                    {
                        var privilege = await _context.UserAdminPrivileges
                            .FirstOrDefaultAsync(p => p.UserId == userId && p.ServerId == sourceBansSetting.ServerId);

                        var endsTimestamp = Convert.ToInt64(adminData["ends"]);
                        var expiresAt = endsTimestamp == 0
                            ? DateTime.MaxValue 
                            : DateTimeOffset.FromUnixTimeSeconds(endsTimestamp).UtcDateTime;

                        if (privilege == null)
                        {
                            privilege = new UserAdminPrivilege
                            {
                                UserId = userId,
                                SteamId = steamId,
                                ServerId = sourceBansSetting.ServerId,
                                Flags = adminData["srv_flags"]?.ToString(),
                                GroupName = adminData["srv_group"]?.ToString(),
                                Immunity = Convert.ToInt32(adminData["immunity"]),
                                StartsAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(adminData["created"])).UtcDateTime,
                                ExpiresAt = expiresAt,
                                IsActive = expiresAt > DateTimeHelper.GetServerLocalTime(),
                                SourceBansAdminId = Convert.ToInt32(adminData["aid"])
                            };
                            _context.UserAdminPrivileges.Add(privilege);
                        }
                        else
                        {
                            privilege.Flags = adminData["srv_flags"]?.ToString();
                            privilege.GroupName = adminData["srv_group"]?.ToString();
                            privilege.Immunity = Convert.ToInt32(adminData["immunity"]);
                            privilege.ExpiresAt = expiresAt;
                            privilege.IsActive = expiresAt > DateTimeHelper.GetServerLocalTime();
                            privilege.SourceBansAdminId = Convert.ToInt32(adminData["aid"]);
                            privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        }

                        hasChanges = true;
                        _logger.LogInformation("Updated admin privilege for user {UserId} on server {ServerId}",
                            userId, sourceBansSetting.ServerId);
                    }
                    else
                    {
                        var privilege = await _context.UserAdminPrivileges
                            .FirstOrDefaultAsync(p => p.UserId == userId && p.ServerId == sourceBansSetting.ServerId && p.IsActive);

                        if (privilege != null)
                        {
                            try
                            {
                                if (privilege.SourceBansAdminId.HasValue)
                                {
                                    var removed = await _sourceBansService.RemoveAdminAsync(privilege.SourceBansAdminId.Value);
                                    if (!removed)
                                    {
                                        _logger.LogWarning("Failed to remove SourceBans admin {AdminId} while syncing; will still remove local record", privilege.SourceBansAdminId.Value);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Error removing SourceBans admin during sync for privilege {PrivilegeId}", privilege.Id);
                            }

                            _context.UserAdminPrivileges.Remove(privilege);
                            hasChanges = true;
                            _logger.LogInformation("Removed local admin privilege for user {UserId} on server {ServerId} during sync", userId, sourceBansSetting.ServerId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing admin status for user {UserId} on server {ServerId}",
                        userId, sourceBansSetting.ServerId);
                }
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }

            return hasChanges;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing admin status for user {UserId}", userId);
            throw;
        }
    }
}

public class AdminStatusDto
{
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public string? Flags { get; set; }
    public string? GroupName { get; set; }
    public int Immunity { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysRemaining { get; set; }
    public DateTime LastSync { get; set; }
}
