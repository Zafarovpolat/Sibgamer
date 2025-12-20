using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Utils;

namespace backend.Services;

public interface IViewTrackingService
{
    Task<bool> TrackNewsViewAsync(int newsId, int? userId, string? ipAddress);
    Task<bool> TrackEventViewAsync(int eventId, int? userId, string? ipAddress);
    Task<bool> TrackCustomPageViewAsync(int customPageId, int? userId, string? ipAddress);
}

public class ViewTrackingService : IViewTrackingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ViewTrackingService> _logger;

    public ViewTrackingService(ApplicationDbContext context, ILogger<ViewTrackingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> TrackNewsViewAsync(int newsId, int? userId, string? ipAddress)
    {
        try
        {
            var today = DateTimeHelper.GetServerLocalTime().Date;
            var tomorrow = today.AddDays(1);

            bool alreadyViewed;

            if (userId.HasValue)
            {
                alreadyViewed = await _context.NewsViews
                    .AnyAsync(v => v.NewsId == newsId 
                        && v.UserId == userId.Value 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else if (!string.IsNullOrEmpty(ipAddress))
            {
                alreadyViewed = await _context.NewsViews
                    .AnyAsync(v => v.NewsId == newsId 
                        && v.IpAddress == ipAddress 
                        && v.UserId == null 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else
            {
                return false;
            }

            if (alreadyViewed)
            {
                return false;
            }

            var view = new NewsView
            {
                NewsId = newsId,
                UserId = userId,
                IpAddress = userId.HasValue ? null : ipAddress, 
                ViewDate = DateTimeHelper.GetServerLocalTime()
            };

            _context.NewsViews.Add(view);

            var news = await _context.News.FindAsync(newsId);
            if (news != null)
            {
                news.ViewCount++;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking news view for NewsId: {NewsId}", newsId);
            return false;
        }
    }

    public async Task<bool> TrackEventViewAsync(int eventId, int? userId, string? ipAddress)
    {
        try
        {
            var today = DateTimeHelper.GetServerLocalTime().Date;
            var tomorrow = today.AddDays(1);

            bool alreadyViewed;

            if (userId.HasValue)
            {
                alreadyViewed = await _context.EventViews
                    .AnyAsync(v => v.EventId == eventId 
                        && v.UserId == userId.Value 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else if (!string.IsNullOrEmpty(ipAddress))
            {
                alreadyViewed = await _context.EventViews
                    .AnyAsync(v => v.EventId == eventId 
                        && v.IpAddress == ipAddress 
                        && v.UserId == null 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else
            {
                return false;
            }

            if (alreadyViewed)
            {
                return false;
            }

            var view = new EventView
            {
                EventId = eventId,
                UserId = userId,
                IpAddress = userId.HasValue ? null : ipAddress, 
                ViewDate = DateTimeHelper.GetServerLocalTime()
            };

            _context.EventViews.Add(view);

            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem != null)
            {
                eventItem.ViewCount++;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking event view for EventId: {EventId}", eventId);
            return false;
        }
    }

    public async Task<bool> TrackCustomPageViewAsync(int customPageId, int? userId, string? ipAddress)
    {
        try
        {
            var today = DateTimeHelper.GetServerLocalTime().Date;
            var tomorrow = today.AddDays(1);

            bool alreadyViewed;

            if (userId.HasValue)
            {
                alreadyViewed = await _context.CustomPageViews
                    .AnyAsync(v => v.CustomPageId == customPageId 
                        && v.UserId == userId.Value 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else if (!string.IsNullOrEmpty(ipAddress))
            {
                alreadyViewed = await _context.CustomPageViews
                    .AnyAsync(v => v.CustomPageId == customPageId 
                        && v.IpAddress == ipAddress 
                        && v.UserId == null 
                        && v.ViewDate >= today 
                        && v.ViewDate < tomorrow);
            }
            else
            {
                return false;
            }

            if (alreadyViewed)
            {
                return false;
            }

            var view = new CustomPageView
            {
                CustomPageId = customPageId,
                UserId = userId,
                IpAddress = userId.HasValue ? null : ipAddress, 
                ViewDate = DateTimeHelper.GetServerLocalTime()
            };

            _context.CustomPageViews.Add(view);

            var customPage = await _context.CustomPages.FindAsync(customPageId);
            if (customPage != null)
            {
                customPage.ViewCount++;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking custom page view for CustomPageId: {CustomPageId}", customPageId);
            return false;
        }
    }
}
