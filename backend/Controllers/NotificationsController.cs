using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(INotificationService notificationService, ApplicationDbContext context, ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _context = context;
        _logger = logger;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool onlyUnread = false)
    {
        try
        {
            var userId = GetUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(userId, page, pageSize, onlyUnread);

            var totalCountQuery = _context.Notifications.Where(n => n.UserId == userId);
            if (onlyUnread)
            {
                totalCountQuery = totalCountQuery.Where(n => !n.IsRead);
            }
            var totalCount = await totalCountQuery.CountAsync();

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                RelatedEntityId = n.RelatedEntityId
            });

            var result = new
            {
                notifications = notificationDtos,
                pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                }
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении уведомлений", error = ex.Message });
        }
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        try
        {
            var userId = GetUserId();
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении количества непрочитанных уведомлений", error = ex.Message });
        }
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var userId = GetUserId();
            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok(new { message = "Уведомление отмечено как прочитанное" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification {NotificationId} as read for user {UserId}", id, User?.Identity?.Name ?? "unknown");
            return StatusCode(500, new { message = "Ошибка при отметке уведомления как прочитанного", error = ex.Message });
        }
    }

    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        try
        {
            var userId = GetUserId();
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "Все уведомления отмечены как прочитанные" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при отметке всех уведомлений как прочитанных", error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        try
        {
            var userId = GetUserId();
            if (id <= 0) return BadRequest(new { message = "Неверный идентификатор уведомления" });
            await _notificationService.DeleteNotificationAsync(id, userId);
            return Ok(new { message = "Уведомление удалено" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification {NotificationId} for user {UserId}", id, User?.Identity?.Name ?? "unknown");
            return StatusCode(500, new { message = "Ошибка при удалении уведомления", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> CreateNotification([FromBody] CreateNotificationDto dto)
    {
        try
        {
            var userId = GetUserId();
            var notification = await _notificationService.CreateNotificationAsync(userId, dto.Title, dto.Message, dto.Type ?? string.Empty, dto.RelatedEntityId);

            var result = new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
                RelatedEntityId = notification.RelatedEntityId
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при создании уведомления", error = ex.Message });
        }
    }
}

public class CreateNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Type { get; set; }
    public int? RelatedEntityId { get; set; }
}

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? RelatedEntityId { get; set; }
}
