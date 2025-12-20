using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/telegram")]
[Authorize(Roles = "Admin")]
public class AdminTelegramController : ControllerBase
{
    private readonly ITelegramService _telegramService;
    private readonly ILogger<AdminTelegramController> _logger;

    public AdminTelegramController(
        ITelegramService telegramService,
        ILogger<AdminTelegramController> logger)
    {
        _telegramService = telegramService;
        _logger = logger;
    }

    [HttpGet("bot-token")]
    public async Task<ActionResult<string?>> GetBotToken()
    {
        try
        {
            var token = await _telegramService.GetBotTokenAsync();
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Telegram bot token");
            return StatusCode(500, new { message = "Ошибка при получении токена бота" });
        }
    }

    [HttpPost("bot-token")]
    public async Task<IActionResult> SetBotToken([FromBody] SetBotTokenDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                return BadRequest(new { message = "Токен не может быть пустым" });
            }

            await _telegramService.SetBotTokenAsync(dto.Token);
            return Ok(new { message = "Токен бота успешно сохранен" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting Telegram bot token");
            return StatusCode(500, new { message = "Ошибка при сохранении токена бота" });
        }
    }

    [HttpPost("send-notification")]
    public async Task<IActionResult> SendNotification([FromBody] SendTelegramNotificationDto dto)
    {
        try
        {
            await _telegramService.SendNotificationToAllAsync(dto.Title, dto.Message);
            return Ok(new { message = "Уведомление отправлено всем подписчикам Telegram" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Telegram notification");
            return StatusCode(500, new { message = "Ошибка при отправке уведомления" });
        }
    }
}

public class SetBotTokenDto
{
    public string Token { get; set; } = string.Empty;
}

public class SendTelegramNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
