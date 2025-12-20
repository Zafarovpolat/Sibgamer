using backend.Data;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Services;

public interface IVipSyncService
{
    Task SyncAllVipsAsync();
    Task SyncUserVipsAsync(int userId);
    Task<List<VipStatusDto>> GetUserVipStatusesAsync(int userId);
    Task<bool> SyncUserVipStatusAsync(int userId, string steamId);
    Task<bool> RemoveVipAsync(int privilegeId);
}

public class VipSyncService : IVipSyncService
{
    private readonly ApplicationDbContext _context;
    private readonly IVipService _vipService;
    private readonly ILogger<VipSyncService> _logger;

    public VipSyncService(
        ApplicationDbContext context,
        IVipService vipService,
        ILogger<VipSyncService> logger)
    {
        _context = context;
        _vipService = vipService;
        _logger = logger;
    }

    public async Task SyncAllVipsAsync()
    {
        _logger.LogInformation("Starting full VIP synchronization");

        try
        {
            var activePrivileges = await _context.UserVipPrivileges
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
                    var isSynced = await SyncUserVipStatusAsync(privilege.UserId, privilege.SteamId);
                    if (isSynced)
                    {
                        syncedCount++;
                        privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing VIP status for user {UserId}, SteamId {SteamId}",
                        privilege.UserId, privilege.SteamId);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("VIP synchronization completed. Synced: {SyncedCount}, Updated: {UpdatedCount}",
                syncedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during full VIP synchronization");
            throw;
        }
    }

    public async Task SyncUserVipsAsync(int userId)
    {
        _logger.LogInformation("Starting VIP synchronization for user {UserId}", userId);

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.SteamId))
            {
                _logger.LogWarning("User {UserId} not found or has no SteamId", userId);
                return;
            }

            await SyncUserVipStatusAsync(userId, user.SteamId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing VIPs for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<VipStatusDto>> GetUserVipStatusesAsync(int userId)
    {
        var privileges = await _context.UserVipPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.IsActive)
            .ThenByDescending(p => p.ExpiresAt)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = new List<VipStatusDto>();

        foreach (var privilege in privileges)
        {
            var status = new VipStatusDto
            {
                ServerId = privilege.ServerId,
                ServerName = privilege.Server?.Name ?? "Unknown Server",
                TariffName = privilege.Tariff?.Name ?? "Unknown Tariff",
                GroupName = privilege.GroupName,
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

    public async Task<bool> SyncUserVipStatusAsync(int userId, string steamId)
    {
        _logger.LogInformation("Syncing VIP status for user {UserId} with SteamId {SteamId}", userId, steamId);

        try
        {
            var vipServers = await _context.VipSettings
                .Where(s => s.IsConfigured)
                .Include(s => s.Server)
                .ToListAsync();

            var hasChanges = false;

            foreach (var vipSetting in vipServers)
            {
                try
                {
                    var vipData = await _vipService.GetVipBySteamIdAsync(steamId);

                    if (vipData != null)
                    {
                        var privilege = await _context.UserVipPrivileges
                            .FirstOrDefaultAsync(p => p.UserId == userId && p.ServerId == vipSetting.ServerId);

                        var endsTimestamp = Convert.ToInt64(vipData["expires"]);
                        var expiresAt = endsTimestamp == 0
                            ? DateTime.MaxValue 
                            : DateTimeOffset.FromUnixTimeSeconds(endsTimestamp).UtcDateTime;

                        var groupName = vipData["group"]?.ToString() ?? "";
                        var vipTariff = await _context.VipTariffs
                            .FirstOrDefaultAsync(t => t.ServerId == vipSetting.ServerId && t.GroupName == groupName);

                        if (privilege == null)
                        {
                            privilege = new UserVipPrivilege
                            {
                                UserId = userId,
                                SteamId = steamId,
                                ServerId = vipSetting.ServerId,
                                TariffId = vipTariff?.Id,
                                GroupName = vipData["group"]?.ToString() ?? "",
                                StartsAt = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(vipData["lastvisit"])).UtcDateTime,
                                ExpiresAt = expiresAt,
                                IsActive = expiresAt > DateTimeHelper.GetServerLocalTime()
                            };
                            _context.UserVipPrivileges.Add(privilege);
                        }
                        else
                        {
                            privilege.GroupName = vipData["group"]?.ToString() ?? "";
                            privilege.ExpiresAt = expiresAt;
                            privilege.IsActive = expiresAt > DateTimeHelper.GetServerLocalTime();
                            privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                            
                            if (!privilege.TariffId.HasValue && vipTariff != null)
                            {
                                privilege.TariffId = vipTariff.Id;
                            }
                        }

                        hasChanges = true;
                        _logger.LogInformation("Updated VIP privilege for user {UserId} on server {ServerId}",
                            userId, vipSetting.ServerId);
                    }
                    else
                    {
                        var privilege = await _context.UserVipPrivileges
                            .FirstOrDefaultAsync(p => p.UserId == userId && p.ServerId == vipSetting.ServerId && p.IsActive);

                        if (privilege != null)
                        {
                            privilege.IsActive = false;
                            privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                            hasChanges = true;
                            _logger.LogInformation("Deactivated VIP privilege for user {UserId} on server {ServerId}",
                                userId, vipSetting.ServerId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing VIP status for user {UserId} on server {ServerId}",
                        userId, vipSetting.ServerId);
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
            _logger.LogError(ex, "Error syncing VIP status for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> RemoveVipAsync(int privilegeId)
    {
        _logger.LogInformation("Removing VIP privilege with ID {PrivilegeId}", privilegeId);

        try
        {
            var privilege = await _context.UserVipPrivileges
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == privilegeId);

            if (privilege == null)
            {
                _logger.LogWarning("VIP privilege with ID {PrivilegeId} not found", privilegeId);
                return false;
            }

            if (!string.IsNullOrEmpty(privilege.SteamId))
            {
                try
                {
                    await _vipService.RemoveVipAsync(privilege.SteamId);
                    _logger.LogInformation("Removed VIP from external database for SteamId {SteamId}", privilege.SteamId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing VIP from external database for SteamId {SteamId}", privilege.SteamId);
                }
            }

            _context.UserVipPrivileges.Remove(privilege);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully removed VIP privilege with ID {PrivilegeId}", privilegeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing VIP privilege with ID {PrivilegeId}", privilegeId);
            throw;
        }
    }
}

public class VipStatusDto
{
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysRemaining { get; set; }
    public DateTime LastSync { get; set; }
}
