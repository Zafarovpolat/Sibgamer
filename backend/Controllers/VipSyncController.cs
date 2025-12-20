using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/vip/sync")]
[Authorize(Roles = "Admin")]
public class VipSyncController : ControllerBase
{
    private readonly IVipSyncService _vipSyncService;

    public VipSyncController(IVipSyncService vipSyncService)
    {
        _vipSyncService = vipSyncService;
    }

    [HttpPost("all")]
    public async Task<IActionResult> SyncAllVips()
    {
        try
        {
            await _vipSyncService.SyncAllVipsAsync();
            return Ok(new { message = "Синхронизация всех VIP завершена" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при синхронизации", error = ex.Message });
        }
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> SyncUserVips(int userId)
    {
        try
        {
            await _vipSyncService.SyncUserVipsAsync(userId);
            return Ok(new { message = $"Синхронизация VIP для пользователя {userId} завершена" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при синхронизации", error = ex.Message });
        }
    }
}

[ApiController]
[Route("api/user")]
[Authorize]
public class UserVipController : ControllerBase
{
    private readonly IVipSyncService _vipSyncService;

    public UserVipController(IVipSyncService vipSyncService)
    {
        _vipSyncService = vipSyncService;
    }

    [HttpGet("vip-status")]
    public async Task<IActionResult> GetMyVipStatuses()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Неверный токен пользователя" });
            }

            var statuses = await _vipSyncService.GetUserVipStatusesAsync(userId);
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении статусов VIP", error = ex.Message });
        }
    }

    [HttpPost("sync-vip-status")]
    public async Task<IActionResult> SyncMyVipStatus()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Неверный токен пользователя" });
            }

            await _vipSyncService.SyncUserVipsAsync(userId);
            return Ok(new { message = "Синхронизация статусов VIP завершена" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при синхронизации", error = ex.Message });
        }
    }
}
