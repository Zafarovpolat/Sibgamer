using backend.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.BackgroundServices;

public class VipSyncBackgroundService : BackgroundService
{
    private readonly ILogger<VipSyncBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _syncInterval = TimeSpan.FromHours(1);

    public VipSyncBackgroundService(
        ILogger<VipSyncBackgroundService> _logger,
        IServiceProvider serviceProvider)
    {
        this._logger = _logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("VIP Sync Background Service is starting");

        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting scheduled VIP synchronization");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var vipSyncService = scope.ServiceProvider.GetRequiredService<IVipSyncService>();
                    await vipSyncService.SyncAllVipsAsync();
                }

                _logger.LogInformation("Scheduled VIP synchronization completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled VIP synchronization");
            }

            await Task.Delay(_syncInterval, stoppingToken);
        }

        _logger.LogInformation("VIP Sync Background Service is stopping");
    }
}
