using backend.Data;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.BackgroundServices;

public class PrivilegeExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrivilegeExpirationService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

    public PrivilegeExpirationService(
        IServiceProvider serviceProvider,
        ILogger<PrivilegeExpirationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrivilegeExpirationService запущен");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiredPrivileges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке истекших привилегий");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

        private async Task CheckExpiredPrivileges()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var sourceBansService = scope.ServiceProvider.GetRequiredService<Services.ISourceBansService>();
        var vipSyncService = scope.ServiceProvider.GetRequiredService<Services.IVipSyncService>();

        var now = DateTimeHelper.GetServerLocalTime();

        var expiredAdminPrivileges = await context.UserAdminPrivileges
            .Where(p => p.IsActive && p.ExpiresAt <= now)
            .ToListAsync();

        if (expiredAdminPrivileges.Any())
        {
            _logger.LogInformation($"Найдено {expiredAdminPrivileges.Count} истекших админ-привилегий");

            foreach (var privilege in expiredAdminPrivileges)
            {
                try
                {
                    // Attempt to remove admin from SourceBans (by stored admin id if exists, otherwise try to find by steam id)
                    if (privilege.SourceBansAdminId.HasValue)
                    {
                        var adminId = privilege.SourceBansAdminId.Value;
                        var removed = await sourceBansService.RemoveAdminAsync(adminId);
                        if (removed)
                        {
                            _logger.LogInformation("Removed external SourceBans admin {AdminId} for local privilege {PrivilegeId}", adminId, privilege.Id);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to remove admin {AdminId} from SourceBans for privilege {PrivilegeId}", adminId, privilege.Id);
                        }
                    }
                    else
                    {
                        // if we don't have admin id, try to find by steam id and remove
                        var adminData = await sourceBansService.GetAdminBySteamIdAsync(privilege.SteamId);
                        if (adminData != null && adminData.TryGetValue("aid", out var aidObj))
                        {
                            var aid = Convert.ToInt32(aidObj);
                            var removed = await sourceBansService.RemoveAdminAsync(aid);
                            if (removed)
                            {
                                _logger.LogInformation("Removed external SourceBans admin {AdminId} (found by steam id) for local privilege {PrivilegeId}", aid, privilege.Id);
                            }
                            else
                            {
                                _logger.LogWarning("Failed to remove external SourceBans admin {AdminId} (found by steam id) for local privilege {PrivilegeId}", aid, privilege.Id);
                            }
                        }
                    }

                    // Always remove local privilege record
                    context.UserAdminPrivileges.Remove(privilege);
                    _logger.LogInformation(
                        $"Удалена локальная админ-привилегия ID={privilege.Id}, Пользователь={privilege.UserId}, Сервер={privilege.ServerId}, Истекла={privilege.ExpiresAt:yyyy-MM-dd HH:mm:ss}"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении админ-привилегии ID={PrivilegeId}", privilege.Id);
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Успешно обработано {expiredAdminPrivileges.Count} админ-привилегий (деактивированы/удалены)");
        }

        var expiredVipPrivileges = await context.UserVipPrivileges
            .Where(p => p.IsActive && p.ExpiresAt <= now)
            .ToListAsync();

        if (expiredVipPrivileges.Any())
        {
            _logger.LogInformation($"Найдено {expiredVipPrivileges.Count} истекших VIP-привилегий");

            foreach (var privilege in expiredVipPrivileges)
            {
                try
                {
                    var removed = await vipSyncService.RemoveVipAsync(privilege.Id);
                    if (removed)
                    {
                        _logger.LogInformation("Removed VIP privilege ID={PrivilegeId} (external + local cleanup)", privilege.Id);
                    }
                    else
                    {
                        privilege.IsActive = false;
                        privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        _logger.LogWarning("Failed to remove VIP privilege ID={PrivilegeId} from external system; marked inactive locally", privilege.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении VIP-привилегии ID={PrivilegeId}", privilege.Id);
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Успешно обработано {expiredVipPrivileges.Count} VIP-привилегий (деактивированы/удалены)");
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PrivilegeExpirationService остановлен");
        return base.StopAsync(cancellationToken);
    }
}
