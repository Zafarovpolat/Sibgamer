using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Cryptography;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthController(ApplicationDbContext context, IJwtService jwtService, IEmailService emailService)
    {
        _context = context;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            return BadRequest(new { message = "Пользователь с таким именем уже существует" });
        }

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new { message = "Email уже используется" });
        }

        var clientIp = IpAddressHelper.GetClientIpAddress(HttpContext);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsAdmin = false,
            LastIp = clientIp
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _ = Task.Run(async () =>
        {
            try
            {
                await _emailService.SendWelcomeEmailAsync(user.Email, user.Username);
            }
            catch
            {
            }
        });

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                SteamId = user.SteamId,
                SteamProfileUrl = user.SteamProfileUrl,
                LastIp = user.LastIp,
                IsAdmin = user.IsAdmin,
                IsBlocked = user.IsBlocked,
                BlockedAt = user.BlockedAt,
                BlockReason = user.BlockReason,
                CreatedAt = user.CreatedAt
            }
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
        }

        if (user.IsBlocked)
        {
            return StatusCode(403, new 
            { 
                error = "account_blocked",
                message = "Ваш аккаунт заблокирован на данном ресурсе",
                reason = user.BlockReason,
                blockedAt = user.BlockedAt,
                blocked = true
            });
        }

        var clientIp = IpAddressHelper.GetClientIpAddress(HttpContext);
        
        if (!string.IsNullOrEmpty(clientIp))
        {
            user.LastIp = clientIp;
        }

        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                SteamId = user.SteamId,
                SteamProfileUrl = user.SteamProfileUrl,
                LastIp = user.LastIp,
                IsAdmin = user.IsAdmin,
                IsBlocked = user.IsBlocked,
                BlockedAt = user.BlockedAt,
                BlockReason = user.BlockReason,
                CreatedAt = user.CreatedAt
            }
        });
    }

    [HttpGet("check-username")]
    public async Task<ActionResult<bool>> CheckUsername([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest(new { message = "Имя пользователя не может быть пустым" });
        }

        var exists = await _context.Users.AnyAsync(u => u.Username == username);
        return Ok(new { available = !exists });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null)
        {
            return Ok(new { message = "Если email существует, мы отправили инструкции по восстановлению пароля" });
        }

        var tokenBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }
        var token = Convert.ToBase64String(tokenBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");

        var resetToken = new PasswordResetToken
        {
            Token = token,
            UserId = user.Id,
            ExpiresAt = DateTimeHelper.GetServerLocalTime().AddHours(1), 
            IsUsed = false,
            CreatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();

        _ = Task.Run(async () =>
        {
            try
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, token);
            }
            catch
            {
            }
        });

        return Ok(new { message = "Если email существует, мы отправили инструкции по восстановлению пароля" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var resetToken = await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == dto.Token && !t.IsUsed);

        if (resetToken == null)
        {
            return BadRequest(new { message = "Неверный или использованный токен" });
        }

        if (resetToken.ExpiresAt < DateTimeHelper.GetServerLocalTime())
        {
            return BadRequest(new { message = "Срок действия токена истек" });
        }

        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        
        resetToken.IsUsed = true;
        resetToken.UsedAt = DateTimeHelper.GetServerLocalTime();

        await _context.SaveChangesAsync();

        return Ok(new { message = "Пароль успешно изменен" });
    }
}
