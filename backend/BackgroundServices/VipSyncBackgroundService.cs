using backend.Services;

namespace backend.BackgroundServices;

public class VipSyncBackgroundService : BackgroundService
{
    private readonly ILogger<VipSyncBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const int STARTUP_DELAY_MINUTES = 12;
    private const int SYNC_INTERVAL_HOURS = 1;
    private const int ERROR_RETRY_DELAY_MINUTES = 5;

    public VipSyncBackgroundService(
        ILogger<VipSyncBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("VipSyncBackgroundService: ожидание {Delay} минут перед стартом...", STARTUP_DELAY_MINUTES);

        try
        {
            await Task.Delay(TimeSpan.FromMinutes(STARTUP_DELAY_MINUTES), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("VipSyncBackgroundService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Запуск синхронизации VIP");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var vipSyncService = scope.ServiceProvider.GetRequiredService<IVipSyncService>();
                    await vipSyncService.SyncAllVipsAsync();
                }

                _logger.LogInformation("Синхронизация VIP завершена");
                await Task.Delay(TimeSpan.FromHours(SYNC_INTERVAL_HOURS), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("VipSyncBackgroundService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при синхронизации VIP");
                
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(ERROR_RETRY_DELAY_MINUTES), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _logger.LogInformation("VipSyncBackgroundService: остановлен");
    }
}