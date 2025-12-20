using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using backend.Data;
using backend.DTOs;
using backend.Services;
using backend.Models;
using backend.Middleware;
using backend.Utils;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly IMemoryCache _cache;

    public AdminUsersController(ApplicationDbContext context, IFileService fileService, IMemoryCache cache)
    {
        _context = context;
        _fileService = fileService;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                SteamId = u.SteamId,
                SteamProfileUrl = u.SteamProfileUrl,
                LastIp = u.LastIp,
                IsAdmin = u.IsAdmin,
                IsBlocked = u.IsBlocked,
                BlockedAt = u.BlockedAt,
                BlockReason = u.BlockReason,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<UserDetailsDto>> GetUserDetails(int id)
    {
        var user = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDetailsDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                SteamId = u.SteamId,
                SteamProfileUrl = u.SteamProfileUrl,
                LastIp = u.LastIp,
                IsAdmin = u.IsAdmin,
                IsBlocked = u.IsBlocked,
                BlockedAt = u.BlockedAt,
                BlockReason = u.BlockReason,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("{id}/toggle-admin")]
    public async Task<IActionResult> ToggleAdmin(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.IsAdmin = !user.IsAdmin;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var authoredNewsCount = await _context.News.CountAsync(n => n.AuthorId == id);
        var authoredEventsCount = await _context.Events.CountAsync(e => e.AuthorId == id);
        var authoredPagesCount = await _context.CustomPages.CountAsync(p => p.AuthorId == id);

        if (authoredNewsCount > 0 || authoredEventsCount > 0 || authoredPagesCount > 0)
        {
            return BadRequest(new
            {
                message = "Нельзя удалить пользователя: у него есть авторские материалы. Удалите или переназначьте их автора сначала.",
                authoredNews = authoredNewsCount,
                authoredEvents = authoredEventsCount,
                authoredPages = authoredPagesCount
            });
        }

        var adminPrivileges = await _context.UserAdminPrivileges
            .Where(p => p.UserId == id)
            .ToListAsync();
        _context.UserAdminPrivileges.RemoveRange(adminPrivileges);

        var vipPrivileges = await _context.UserVipPrivileges
            .Where(p => p.UserId == id)
            .ToListAsync();
        _context.UserVipPrivileges.RemoveRange(vipPrivileges);

        var transactions = await _context.DonationTransactions
            .Where(t => t.UserId == id)
            .ToListAsync();
        foreach (var transaction in transactions)
        {
            transaction.UserId = null;
        }

        var notifications = await _context.Notifications
            .Where(n => n.UserId == id)
            .ToListAsync();
        _context.Notifications.RemoveRange(notifications);

        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            await _fileService.DeleteFileAsync(user.AvatarUrl);
        }

        _context.Users.Remove(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return Problem(detail: ex.Message, statusCode: 500);
        }

        return NoContent();
    }

    [HttpPost("{id}/block")]
    public async Task<IActionResult> BlockUser(int id, [FromBody] BlockUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(adminIdClaim, out var adminId))
        {
            return Unauthorized();
        }

        user.IsBlocked = true;
        user.BlockedAt = DateTimeHelper.GetServerLocalTime();
        user.BlockReason = dto.Reason ?? "Заблокирован администратором";

        var newsComments = await _context.NewsComments
            .Where(c => c.UserId == id)
            .ToListAsync();
        _context.NewsComments.RemoveRange(newsComments);

        var eventComments = await _context.EventComments
            .Where(c => c.UserId == id)
            .ToListAsync();
        _context.EventComments.RemoveRange(eventComments);

        var donationTransactions = await _context.DonationTransactions
            .Where(t => t.UserId == id)
            .ToListAsync();
        _context.DonationTransactions.RemoveRange(donationTransactions);

        if (!string.IsNullOrEmpty(user.LastIp))
        {
            var existingBlock = await _context.BlockedIps
                .FirstOrDefaultAsync(b => b.IpAddress == user.LastIp);

            if (existingBlock == null)
            {
                var blockedIp = new BlockedIp
                {
                    IpAddress = user.LastIp,
                    Reason = dto.Reason ?? "Заблокирован администратором",
                    BlockedByUserId = adminId
                };
                _context.BlockedIps.Add(blockedIp);

                IpBlockMiddleware.ClearIpCache(_cache, user.LastIp);
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new 
        { 
            message = "Пользователь успешно заблокирован",
            blockedAt = user.BlockedAt
        });
    }

    [HttpPost("{id}/unblock")]
    public async Task<IActionResult> UnblockUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.IsBlocked = false;
        user.BlockedAt = null;
        user.BlockReason = null;

        if (!string.IsNullOrEmpty(user.LastIp))
        {
            var blockedIp = await _context.BlockedIps
                .FirstOrDefaultAsync(b => b.IpAddress == user.LastIp);

            if (blockedIp != null)
            {
                _context.BlockedIps.Remove(blockedIp);

                IpBlockMiddleware.ClearIpCache(_cache, user.LastIp);
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new 
        { 
            message = "Пользователь успешно разблокирован"
        });
    }
}
