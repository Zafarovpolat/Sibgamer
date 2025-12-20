using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/email")]
[Authorize(Roles = "Admin")]
public class AdminEmailController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<AdminEmailController> _logger;

    public AdminEmailController(
        ApplicationDbContext context, 
        IEmailService emailService,
        ILogger<AdminEmailController> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpGet("settings")]
    public async Task<ActionResult<SmtpSettingsDto>> GetSettings()
    {

        var settings = await _context.SmtpSettings
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            return Ok(new SmtpSettingsDto
            {
                Id = 0,
                Host = "",
                Port = 587,
                Username = "",
                Password = "",
                EnableSsl = true,
                FromEmail = "",
                FromName = "",
                IsConfigured = false,
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            });
        }

        return Ok(new SmtpSettingsDto
        {
            Id = settings.Id,
            Host = settings.Host,
            Port = settings.Port,
            Username = settings.Username,
            Password = settings.Password,
            EnableSsl = settings.EnableSsl,
            FromEmail = settings.FromEmail,
            FromName = settings.FromName,
            IsConfigured = settings.IsConfigured,
            UpdatedAt = settings.UpdatedAt
        });
    }

    [HttpPut("settings")]
    public async Task<ActionResult<SmtpSettingsDto>> UpdateSettings([FromBody] UpdateSmtpSettingsDto dto)
    {
        try
        {
            var existingSettings = await _context.SmtpSettings
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            SmtpSettings settings;

        if (existingSettings != null)
        {
            existingSettings.Host = dto.Host;
            existingSettings.Port = dto.Port;
            existingSettings.Username = dto.Username;
            existingSettings.Password = dto.Password;
            existingSettings.EnableSsl = dto.EnableSsl;
            existingSettings.FromEmail = dto.FromEmail;
            existingSettings.FromName = dto.FromName;
            existingSettings.IsConfigured = true;
            existingSettings.UpdatedAt = DateTimeHelper.GetServerLocalTime();

            settings = existingSettings;
        }
        else
        {
            settings = new SmtpSettings
            {
                Host = dto.Host,
                Port = dto.Port,
                Username = dto.Username,
                Password = dto.Password,
                EnableSsl = dto.EnableSsl,
                FromEmail = dto.FromEmail,
                FromName = dto.FromName,
                IsConfigured = true,
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            };

            _context.SmtpSettings.Add(settings);
        }

        await _context.SaveChangesAsync();

        return Ok(new SmtpSettingsDto
        {
            Id = settings.Id,
            Host = settings.Host,
            Port = settings.Port,
            Username = settings.Username,
            Password = settings.Password,
            EnableSsl = settings.EnableSsl,
            FromEmail = settings.FromEmail,
            FromName = settings.FromName,
            IsConfigured = settings.IsConfigured,
            UpdatedAt = settings.UpdatedAt
        });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SMTP settings");
            return StatusCode(500, new { message = "Ошибка при сохранении настроек" });
        }
    }

    [HttpPost("test")]
    public async Task<ActionResult<TestEmailResponseDto>> TestConnection([FromBody] TestEmailRequestDto request)
    {
        try
        {
            var (success, message) = await _emailService.TestConnectionAsync(request.TestEmailAddress);

            return Ok(new TestEmailResponseDto
            {
                Success = success,
                Message = message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing email connection");
            return Ok(new TestEmailResponseDto
            {
                Success = false,
                Message = "Ошибка при тестировании подключения"
            });
        }
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<BulkEmailResponseDto>> SendBulkEmail([FromBody] BulkEmailDto dto)
    {
        var totalUsers = await _context.Users.CountAsync(u => !string.IsNullOrEmpty(u.Email));

        _logger.LogInformation("Starting bulk email send to {Count} users", totalUsers);

        var (successCount, failureCount, errors) = await _emailService.SendBulkEmailAsync(dto.Subject, dto.Body);

        _logger.LogInformation("Bulk email completed. Success: {Success}, Failures: {Failures}", 
            successCount, failureCount);

        return Ok(new BulkEmailResponseDto
        {
            TotalRecipients = totalUsers,
            SuccessCount = successCount,
            FailureCount = failureCount,
            Errors = errors
        });
    }
}
