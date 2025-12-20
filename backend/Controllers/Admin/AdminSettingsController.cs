using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Utils;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class SettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, string>>> GetSettings([FromQuery] string? category = null)
    {
        var query = _context.SiteSettings.AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(s => s.Category == category);
        }

        var settings = await query.OrderBy(s => s.Category).ThenBy(s => s.Key).ToListAsync();
        var result = settings.ToDictionary(s => s.Key, s => s.Value);
        return Ok(result);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<SiteSetting>> GetSetting(string key)
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
        {
            return NotFound(new { message = "Настройка не найдена" });
        }

        return Ok(setting);
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> UpdateSetting(string key, [FromBody] UpdateSettingDto dto)
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
        {
            return NotFound(new { message = "Настройка не найдена" });
        }

        setting.Value = dto.Value;
        setting.UpdatedAt = DateTimeHelper.GetServerLocalTime();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Ошибка при обновлении настройки" });
        }

        return Ok(setting);
    }

    [HttpPost]
    public async Task<ActionResult<SiteSetting>> CreateSetting([FromBody] CreateSettingDto dto)
    {
        var existingSetting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == dto.Key);
        if (existingSetting != null)
        {
            return BadRequest(new { message = "Настройка с таким ключом уже существует" });
        }

        var setting = new SiteSetting
        {
            Key = dto.Key,
            Value = dto.Value,
            Category = dto.Category,
            Description = dto.Description ?? string.Empty,
            DataType = dto.DataType ?? "string",
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.SiteSettings.Add(setting);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Ошибка при создании настройки" });
        }

        return CreatedAtAction(nameof(GetSetting), new { key = setting.Key }, setting);
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteSetting(string key)
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
        {
            return NotFound(new { message = "Настройка не найдена" });
        }

        _context.SiteSettings.Remove(setting);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("batch")]
    public async Task<IActionResult> UpdateSettingsBatch([FromBody] List<UpdateSettingDto> settings)
    {
        foreach (var dto in settings)
        {
            var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == dto.Key);

            if (setting != null)
            {
                setting.Value = dto.Value;
                setting.UpdatedAt = DateTimeHelper.GetServerLocalTime();
            }
            else
            {
                var newSetting = new SiteSetting
                {
                    Key = dto.Key,
                    Value = dto.Value,
                    Category = string.Empty,
                    Description = string.Empty,
                    DataType = "string",
                    CreatedAt = DateTimeHelper.GetServerLocalTime(),
                    UpdatedAt = DateTimeHelper.GetServerLocalTime()
                };

                _context.SiteSettings.Add(newSetting);
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Ошибка при обновлении настроек" });
        }

        return Ok(new { message = "Настройки успешно обновлены" });
    }
}

public class CreateSettingDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DataType { get; set; }
}

public class UpdateSettingDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
