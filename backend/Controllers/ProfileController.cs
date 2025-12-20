using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Services;
using backend.Utils;
using System.Security.Claims;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly ISourceBansService _sourceBansService;

    public ProfileController(ApplicationDbContext context, IFileService fileService, ISourceBansService sourceBansService)
    {
        _context = context;
        _fileService = fileService;
        _sourceBansService = sourceBansService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userId = GetUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SteamId = user.SteamId,
            SteamProfileUrl = user.SteamProfileUrl,
            IsAdmin = user.IsAdmin,
            CreatedAt = user.CreatedAt
        });
    }

    [HttpPut("username")]
    public async Task<ActionResult<UserDto>> UpdateUsername(UpdateUsernameDto dto)
    {
        var userId = GetUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }

        if (await _context.Users.AnyAsync(u => u.Username == dto.NewUsername && u.Id != userId))
        {
            return BadRequest(new { message = "Это имя пользователя уже занято" });
        }

        user.Username = dto.NewUsername;
        await _context.SaveChangesAsync();

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SteamId = user.SteamId,
            SteamProfileUrl = user.SteamProfileUrl,
            IsAdmin = user.IsAdmin,
            CreatedAt = user.CreatedAt
        });
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordDto dto)
    {
        var userId = GetUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            return BadRequest(new { message = "Неверный текущий пароль" });
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Пароль успешно изменен" });
    }

    [HttpPut("avatar")]
    public async Task<ActionResult<UserDto>> UpdateAvatar([FromBody] UpdateAvatarDto dto)
    {
        var userId = GetUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }

        if (!string.IsNullOrEmpty(user.AvatarUrl) && user.AvatarUrl != dto.AvatarUrl)
        {
            await _fileService.DeleteFileAsync(user.AvatarUrl);
        }

        user.AvatarUrl = dto.AvatarUrl;
        await _context.SaveChangesAsync();

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SteamId = user.SteamId,
            SteamProfileUrl = user.SteamProfileUrl,
            IsAdmin = user.IsAdmin,
            CreatedAt = user.CreatedAt
        });
    }

    [HttpPut("admin-password")]
    public async Task<IActionResult> UpdateAdminPassword([FromBody] UpdateAdminPasswordDto dto)
    {
        var userId = GetUserId();
        
        var privilege = await _context.UserAdminPrivileges
            .Include(p => p.Server)
            .FirstOrDefaultAsync(p => p.Id == dto.PrivilegeId && p.UserId == userId && p.IsActive);

        if (privilege == null)
        {
            return NotFound(new { message = "Привилегия не найдена или не активна" });
        }

        if (privilege.SourceBansAdminId == null)
        {
            return BadRequest(new { message = "Привилегия не синхронизирована с SourceBans" });
        }

        var success = await _sourceBansService.UpdateAdminPasswordAsync(privilege.SourceBansAdminId.Value, dto.NewPassword);
        if (!success)
        {
            return BadRequest(new { message = "Не удалось обновить пароль в SourceBans" });
        }

        privilege.AdminPassword = dto.NewPassword;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Пароль админа успешно изменен" });
    }

    [HttpPut("steam")]
    public async Task<ActionResult<UserDto>> UpdateSteamId([FromBody] UpdateSteamIdDto dto)
    {
        var userId = GetUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "Пользователь не найден" });
        }

        var (success, steamId, profileUrl, error) = SteamIdConverter.ConvertToSteamFormat(dto.SteamInput);

        if (!success || steamId == null || profileUrl == null)
        {
            return BadRequest(new { message = error ?? "Неверный формат Steam ID" });
        }

        user.SteamId = steamId;
        user.SteamProfileUrl = profileUrl;
        await _context.SaveChangesAsync();

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SteamId = user.SteamId,
            SteamProfileUrl = user.SteamProfileUrl,
            IsAdmin = user.IsAdmin,
            CreatedAt = user.CreatedAt
        });
    }
}
