using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, string>>> GetPublicSettings([FromQuery] string? category = null)
    {
        var query = _context.SiteSettings.AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(s => s.Category == category);
        }

        var settings = await query.ToListAsync();
        var result = settings.ToDictionary(s => s.Key, s => s.Value);

        return Ok(result);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<string>> GetPublicSetting(string key)
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
        {
            return NotFound(new { message = "Настройка не найдена" });
        }

        return Ok(new { key = setting.Key, value = setting.Value });
    }
}
