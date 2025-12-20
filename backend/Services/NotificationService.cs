using backend.Data;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(int userId, string title, string message, string type, int? relatedEntityId = null);
    Task CreateAdminNotificationAsync(string title, string message, string type, int? relatedEntityId = null);
    Task<List<Notification>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 20, bool onlyUnread = false);
    Task<int> GetUnreadCountAsync(int userId);
    Task MarkAsReadAsync(int notificationId, int userId);
    Task MarkAllAsReadAsync(int userId);
    Task DeleteNotificationAsync(int notificationId, int userId);
}

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Notification> CreateNotificationAsync(int userId, string title, string message, string type, int? relatedEntityId = null)
    {
        try
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                RelatedEntityId = relatedEntityId,
                IsRead = false,
                CreatedAt = DateTimeHelper.GetServerLocalTime()
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created notification for user {UserId}: {Title}", userId, title);

            return notification;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification for user {UserId}", userId);
            throw;
        }
    }

    public async Task CreateAdminNotificationAsync(string title, string message, string type, int? relatedEntityId = null)
    {
        try
        {
            var adminUsers = await _context.Users
                .Where(u => u.IsAdmin)
                .ToListAsync();

            foreach (var admin in adminUsers)
            {
                await CreateNotificationAsync(admin.Id, title, message, type, relatedEntityId);
            }

            _logger.LogInformation("Created admin notification for {Count} admins: {Title}", adminUsers.Count, title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating admin notification");
            throw;
        }
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 20, bool onlyUnread = false)
    {
        try
        {
            var skip = (page - 1) * pageSize;

            var query = _context.Notifications
                .Where(n => n.UserId == userId);

            if (onlyUnread)
            {
                query = query.Where(n => !n.IsRead);
            }

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notifications for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        try
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
            return 0;
        }
    }

    public async Task MarkAsReadAsync(int notificationId, int userId)
    {
        try
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Marked notification {NotificationId} as read for user {UserId}", notificationId, userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification {NotificationId} as read for user {UserId}", notificationId, userId);
            throw;
        }
    }

    public async Task MarkAllAsReadAsync(int userId)
    {
        try
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Marked {Count} notifications as read for user {UserId}", unreadNotifications.Count, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read for user {UserId}", userId);
            throw;
        }
    }

    public async Task DeleteNotificationAsync(int notificationId, int userId)
    {
        try
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted notification {NotificationId} for user {UserId}", notificationId, userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification {NotificationId} for user {UserId}", notificationId, userId);
            throw;
        }
    }
}
