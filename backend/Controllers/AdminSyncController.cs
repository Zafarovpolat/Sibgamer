using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/admin/sync")]
[Authorize(Roles = "Admin")]
public class AdminSyncController : ControllerBase
{
    private readonly IAdminSyncService _adminSyncService;

    public AdminSyncController(IAdminSyncService adminSyncService)
    {
        _adminSyncService = adminSyncService;
    }

    [HttpPost("all")]
    public async Task<IActionResult> SyncAllAdmins()
    {
        try
        {
            await _adminSyncService.SyncAllAdminsAsync();
            return Ok(new { message = "Синхронизация всех админов завершена" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при синхронизации", error = ex.Message });
        }
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> SyncUserAdmins(int userId)
    {
        try
        {
            await _adminSyncService.SyncUserAdminsAsync(userId);
            return Ok(new { message = $"Синхронизация админов для пользователя {userId} завершена" });
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
public class UserAdminController : ControllerBase
{
    private readonly IAdminSyncService _adminSyncService;

    public UserAdminController(IAdminSyncService adminSyncService)
    {
        _adminSyncService = adminSyncService;
    }


    [HttpGet("admin-status")]
    public async Task<IActionResult> GetMyAdminStatuses()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Неверный токен пользователя" });
            }

            var statuses = await _adminSyncService.GetUserAdminStatusesAsync(userId);
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении статусов админа", error = ex.Message });
        }
    }

    [HttpPost("sync-admin-status")]
    public async Task<IActionResult> SyncMyAdminStatus()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Неверный токен пользователя" });
            }

            await _adminSyncService.SyncUserAdminsAsync(userId);
            return Ok(new { message = "Синхронизация статусов админа завершена" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при синхронизации", error = ex.Message });
        }
    }
}
