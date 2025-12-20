using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using backend.Utils;

namespace backend.BackgroundServices;

public class ServerMonitoringService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ServerMonitoringService> _logger;

    public ServerMonitoringService(
        IServiceProvider serviceProvider,
        ILogger<ServerMonitoringService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Server Monitoring Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateServersStatusAsync();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Server Monitoring Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Server Monitoring Service");
                
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Server Monitoring Service is stopping");
                    break;
                }
            }
        }
        
        _logger.LogInformation("Server Monitoring Service stopped");
    }

    private async Task UpdateServersStatusAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var queryService = scope.ServiceProvider.GetRequiredService<IServerQueryService>();

        var servers = await context.Servers.ToListAsync();

        foreach (var server in servers)
        {
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

                    _logger.LogInformation(
                        "Updated server {ServerName}: {Players}/{MaxPlayers} on {Map} - {Status}",
                        server.Name, server.CurrentPlayers, server.MaxPlayers, server.MapName,
                        server.IsOnline ? "Online" : "Offline");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying server {IpAddress}:{Port}", 
                    server.IpAddress, server.Port);
                server.IsOnline = false;
            }
        }

        await context.SaveChangesAsync();
    }
}
