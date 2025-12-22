using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Utils;

namespace backend.BackgroundServices;

public class ServerMonitoringService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ServerMonitoringService> _logger;
    private const int STARTUP_DELAY_SECONDS = 60;
    private const int CHECK_INTERVAL_MINUTES = 5;
    private const int ERROR_RETRY_DELAY_SECONDS = 60;

    public ServerMonitoringService(
        IServiceProvider serviceProvider,
        ILogger<ServerMonitoringService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ServerMonitoringService: ожидание {Delay} секунд перед стартом...", STARTUP_DELAY_SECONDS);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(STARTUP_DELAY_SECONDS), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("ServerMonitoringService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateServersStatusAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ServerMonitoringService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ServerMonitoringService");
                
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
        
        _logger.LogInformation("ServerMonitoringService: остановлен");
    }

    private async Task UpdateServersStatusAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var queryService = scope.ServiceProvider.GetRequiredService<IServerQueryService>();

        var servers = await context.Servers.ToListAsync(stoppingToken);

        if (!servers.Any())
        {
            _logger.LogDebug("Нет серверов для мониторинга");
            return;
        }

        foreach (var server in servers)
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                var result = await queryService.QueryServerAsync(server.IpAddress, server.Port);

                if (result != null)
                {
                    server.Name = result.ServerName;
                    server.MapName = result.Map;
                    server.CurrentPlayers = result.Players;
                    server.MaxPlayers = result.MaxPlayers;
                    server.IsOnline = result.IsOnline;
                    server.LastChecked = DateTimeHelper.GetServerLocalTime();

                    _logger.LogDebug(
                        "Updated server {ServerName}: {Players}/{MaxPlayers} on {Map} - {Status}",
                        server.Name, server.CurrentPlayers, server.MaxPlayers, server.MapName,
                        server.IsOnline ? "Online" : "Offline");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error querying server {IpAddress}:{Port}", 
                    server.IpAddress, server.Port);
                server.IsOnline = false;
                server.LastChecked = DateTimeHelper.GetServerLocalTime();
            }
        }

        await context.SaveChangesAsync(stoppingToken);
    }
}