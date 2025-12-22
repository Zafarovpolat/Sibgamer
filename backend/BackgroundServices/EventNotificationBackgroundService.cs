using backend.Data;
using backend.Models;
using backend.Services;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.BackgroundServices;

public class EventNotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventNotificationBackgroundService> _logger;
    private const int STARTUP_DELAY_SECONDS = 45; // Ждём пока БД "проснётся"
    private const int ERROR_RETRY_DELAY_SECONDS = 60;
    private const int CHECK_INTERVAL_MINUTES = 5;

    public EventNotificationBackgroundService(IServiceProvider serviceProvider, ILogger<EventNotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Отложенный старт - даём время на инициализацию БД
        _logger.LogInformation("EventNotificationBackgroundService: ожидание {Delay} секунд перед стартом...", STARTUP_DELAY_SECONDS);
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(STARTUP_DELAY_SECONDS), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("EventNotificationBackgroundService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessNotificationsAsync(stoppingToken);
                
                // Фиксированный интервал проверки вместо динамического
                await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("EventNotificationBackgroundService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EventNotificationBackgroundService");
                
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
    }

    private async Task ProcessNotificationsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

        var now = DateTimeHelper.GetServerLocalTime();

        // Уведомления о начале событий
        var startingEvents = await context.Events
            .Where(e => e.IsPublished && 
                       !e.IsStartNotificationSent && 
                       e.StartDate <= now)
            .ToListAsync(stoppingToken);

        foreach (var eventItem in startingEvents)
        {
            if (stoppingToken.IsCancellationRequested) break;
            
            try
            {
                await telegramService.SendEventStartedNotificationAsync(eventItem);
                eventItem.IsStartNotificationSent = true;
                await context.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Sent start notification for event {EventId}: {Title}", eventItem.Id, eventItem.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send start notification for event {EventId}", eventItem.Id);
            }
        }

        // Уведомления о завершении событий
        var endingEvents = await context.Events
            .Where(e => e.IsPublished && 
                       !e.IsEndNotificationSent && 
                       e.EndDate <= now)
            .ToListAsync(stoppingToken);

        foreach (var eventItem in endingEvents)
        {
            if (stoppingToken.IsCancellationRequested) break;
            
            try
            {
                await telegramService.SendEventEndedNotificationAsync(eventItem);
                eventItem.IsEndNotificationSent = true;
                await context.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Sent end notification for event {EventId}: {Title}", eventItem.Id, eventItem.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send end notification for event {EventId}", eventItem.Id);
            }
        }

        if (startingEvents.Any() || endingEvents.Any())
        {
            _logger.LogInformation("Processed {StartCount} start and {EndCount} end notifications", 
                startingEvents.Count, endingEvents.Count);
        }
    }
}