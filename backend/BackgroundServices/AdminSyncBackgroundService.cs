using backend.Services;

namespace backend.BackgroundServices;

public class AdminSyncBackgroundService : BackgroundService
{
    private readonly ILogger<AdminSyncBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const int STARTUP_DELAY_MINUTES = 10;
    private const int SYNC_INTERVAL_HOURS = 1;
    private const int ERROR_RETRY_DELAY_MINUTES = 5;

    public AdminSyncBackgroundService(
        ILogger<AdminSyncBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AdminSyncBackgroundService: ожидание {Delay} минут перед стартом...", STARTUP_DELAY_MINUTES);

        try
        {
            await Task.Delay(TimeSpan.FromMinutes(STARTUP_DELAY_MINUTES), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("AdminSyncBackgroundService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Запуск синхронизации админов");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var adminSyncService = scope.ServiceProvider.GetRequiredService<IAdminSyncService>();
                    await adminSyncService.SyncAllAdminsAsync();
                }

                _logger.LogInformation("Синхронизация админов завершена");
                await Task.Delay(TimeSpan.FromHours(SYNC_INTERVAL_HOURS), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("AdminSyncBackgroundService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при синхронизации админов");
                
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

        _logger.LogInformation("AdminSyncBackgroundService: остановлен");
    }
}