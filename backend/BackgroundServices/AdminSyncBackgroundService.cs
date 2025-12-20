using backend.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.BackgroundServices;

public class AdminSyncBackgroundService : BackgroundService
{
    private readonly ILogger<AdminSyncBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _syncInterval = TimeSpan.FromHours(1); 

    public AdminSyncBackgroundService(
        ILogger<AdminSyncBackgroundService> _logger,
        IServiceProvider serviceProvider)
    {
        this._logger = _logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Admin Sync Background Service is starting");

        await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting scheduled admin synchronization");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var adminSyncService = scope.ServiceProvider.GetRequiredService<IAdminSyncService>();
                    await adminSyncService.SyncAllAdminsAsync();
                }

                _logger.LogInformation("Scheduled admin synchronization completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled admin synchronization");
            }

            await Task.Delay(_syncInterval, stoppingToken);
        }

        _logger.LogInformation("Admin Sync Background Service is stopping");
    }
}
