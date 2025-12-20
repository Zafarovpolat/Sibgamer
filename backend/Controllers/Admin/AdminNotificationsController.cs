using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Utils;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/notifications")]
[Authorize(Roles = "Admin")]
public class AdminNotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminNotificationsController> _logger;

    public AdminNotificationsController(
        ApplicationDbContext context,
        ILogger<AdminNotificationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("send-to-all")]
    public async Task<ActionResult<SendNotificationResponseDto>> SendToAllUsers([FromBody] SendNotificationDto dto)
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            var totalRecipients = users.Count;

            if (totalRecipients == 0)
            {
                return Ok(new SendNotificationResponseDto
                {
                    TotalRecipients = 0,
                    SuccessCount = 0,
                    Message = "Нет пользователей для отправки уведомлений"
                });
            }

            var notifications = users.Select(user => new Notification
            {
                UserId = user.Id,
                Title = dto.Title,
                Message = dto.Message,
                Type = "admin_notification",
                IsRead = false,
                CreatedAt = DateTimeHelper.GetServerLocalTime()
            }).ToList();

            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Admin notification sent to {Count} users. Title: {Title}",
                totalRecipients, dto.Title);

            return Ok(new SendNotificationResponseDto
            {
                TotalRecipients = totalRecipients,
                SuccessCount = totalRecipients,
                Message = $"Уведомление успешно отправлено {totalRecipients} пользователям"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending admin notification to all users");
            return StatusCode(500, new SendNotificationResponseDto
            {
                TotalRecipients = 0,
                SuccessCount = 0,
                Message = "Ошибка при отправке уведомлений"
            });
        }
    }
}
