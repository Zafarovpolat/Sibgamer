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

    public EventNotificationBackgroundService(IServiceProvider serviceProvider, ILogger<EventNotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

                    var now = DateTimeHelper.GetServerLocalTime();

                    var startingEvents = await context.Events
                        .Where(e => e.IsPublished && 
                                   !e.IsStartNotificationSent && 
                                   e.StartDate <= now)
                        .ToListAsync();

                    foreach (var eventItem in startingEvents)
                    {
                        try
                        {
                            await telegramService.SendEventStartedNotificationAsync(eventItem);
                            eventItem.IsStartNotificationSent = true;
                            await context.SaveChangesAsync();
                            _logger.LogInformation("Sent start notification for event {EventId}: {Title}", eventItem.Id, eventItem.Title);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send start notification for event {EventId}", eventItem.Id);
                        }
                    }

                    var endingEvents = await context.Events
                        .Where(e => e.IsPublished && 
                                   !e.IsEndNotificationSent && 
                                   e.EndDate <= now)
                        .ToListAsync();

                    foreach (var eventItem in endingEvents)
                    {
                        try
                        {
                            await telegramService.SendEventEndedNotificationAsync(eventItem);
                            eventItem.IsEndNotificationSent = true;
                            await context.SaveChangesAsync();
                            _logger.LogInformation("Sent end notification for event {EventId}: {Title}", eventItem.Id, eventItem.Title);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send end notification for event {EventId}", eventItem.Id);
                        }
                    }

                    var nextEventTime = await GetNextEventTimeAsync(context, now);
                    if (nextEventTime.HasValue)
                    {
                        var delay = nextEventTime.Value - now;
                        if (delay > TimeSpan.Zero)
                        {
                            var maxDelay = TimeSpan.FromMinutes(1); 
                            if (delay > maxDelay)
                            {
                                _logger.LogWarning("Next event is far in the future ({Delay}). Clamping wait to {MaxDelay} and will re-evaluate sooner.", delay, maxDelay);
                                await Task.Delay(maxDelay, stoppingToken);
                            }
                            else
                            {
                                _logger.LogInformation("Waiting until {NextTime} for next event check", nextEventTime.Value);
                                await Task.Delay(delay, stoppingToken);
                            }
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No upcoming events, waiting 1 minute");
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("EventNotificationBackgroundService cancellation requested; stopping service.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EventNotificationBackgroundService");
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); 
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("EventNotificationBackgroundService cancellation requested during error delay; stopping service.");
                    break;
                }
            }
        }
    }

    private async Task<DateTime?> GetNextEventTimeAsync(ApplicationDbContext context, DateTime now)
    {
        var nextStart = await context.Events
            .Where(e => e.IsPublished && !e.IsStartNotificationSent && e.StartDate > now)
            .OrderBy(e => e.StartDate)
            .Select(e => (DateTime?)e.StartDate)
            .FirstOrDefaultAsync();

        var nextEnd = await context.Events
            .Where(e => e.IsPublished && !e.IsEndNotificationSent && e.EndDate > now)
            .OrderBy(e => e.EndDate)
            .Select(e => (DateTime?)e.EndDate)
            .FirstOrDefaultAsync();

        if (nextStart.HasValue && nextEnd.HasValue)
        {
            return nextStart.Value < nextEnd.Value ? nextStart.Value : nextEnd.Value;
        }
        else if (nextStart.HasValue)
        {
            return nextStart.Value;
        }
        else if (nextEnd.HasValue)
        {
            return nextEnd.Value;
        }
        else
        {
            return null;
        }
    }
}
