using backend.Data;
using backend.Utils;
using backend.Services;
using Microsoft.EntityFrameworkCore;

namespace backend.BackgroundServices;

public class PrivilegeExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrivilegeExpirationService> _logger;
    private const int STARTUP_DELAY_SECONDS = 90;
    private const int CHECK_INTERVAL_MINUTES = 5;
    private const int ERROR_RETRY_DELAY_SECONDS = 60;

    public PrivilegeExpirationService(
        IServiceProvider serviceProvider,
        ILogger<PrivilegeExpirationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrivilegeExpirationService: ожидание {Delay} секунд перед стартом...", STARTUP_DELAY_SECONDS);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(STARTUP_DELAY_SECONDS), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("PrivilegeExpirationService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiredPrivileges(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("PrivilegeExpirationService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке истекших привилегий");
                
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(ERROR_RETRY_DELAY_SECONDS), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _logger.LogInformation("PrivilegeExpirationService: остановлен");
    }

    private async Task CheckExpiredPrivileges(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var sourceBansService = scope.ServiceProvider.GetRequiredService<ISourceBansService>();
        var vipSyncService = scope.ServiceProvider.GetRequiredService<IVipSyncService>();

        var now = DateTimeHelper.GetServerLocalTime();

        // Обработка Admin привилегий
        var expiredAdminPrivileges = await context.UserAdminPrivileges
            .Where(p => p.IsActive && p.ExpiresAt <= now)
            .ToListAsync(stoppingToken);

        if (expiredAdminPrivileges.Any())
        {
            _logger.LogInformation("Найдено {Count} истекших админ-привилегий", expiredAdminPrivileges.Count);

            foreach (var privilege in expiredAdminPrivileges)
            {
                if (stoppingToken.IsCancellationRequested) break;

                try
                {
                    if (privilege.SourceBansAdminId.HasValue)
                    {
                        var adminId = privilege.SourceBansAdminId.Value;
                        var removed = await sourceBansService.RemoveAdminAsync(adminId);
                        if (removed)
                        {
                            _logger.LogInformation("Removed SourceBans admin {AdminId} for privilege {PrivilegeId}", adminId, privilege.Id);
                        }
                    }
                    else
                    {
                        var adminData = await sourceBansService.GetAdminBySteamIdAsync(privilege.SteamId);
                        if (adminData != null && adminData.TryGetValue("aid", out var aidObj))
                        {
                            var aid = Convert.ToInt32(aidObj);
                            await sourceBansService.RemoveAdminAsync(aid);
                        }
                    }

                    context.UserAdminPrivileges.Remove(privilege);
                    _logger.LogInformation("Удалена админ-привилегия ID={PrivilegeId}", privilege.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении админ-привилегии ID={PrivilegeId}", privilege.Id);
                }
            }

            await context.SaveChangesAsync(stoppingToken);
        }

        // Обработка VIP привилегий
        var expiredVipPrivileges = await context.UserVipPrivileges
            .Where(p => p.IsActive && p.ExpiresAt <= now)
            .ToListAsync(stoppingToken);

        if (expiredVipPrivileges.Any())
        {
            _logger.LogInformation("Найдено {Count} истекших VIP-привилегий", expiredVipPrivileges.Count);

            foreach (var privilege in expiredVipPrivileges)
            {
                if (stoppingToken.IsCancellationRequested) break;

                try
                {
                    var removed = await vipSyncService.RemoveVipAsync(privilege.Id);
                    if (!removed)
                    {
                        privilege.IsActive = false;
                        privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        _logger.LogWarning("VIP {PrivilegeId} отмечен неактивным локально", privilege.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при удалении VIP-привилегии ID={PrivilegeId}", privilege.Id);
                }
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}