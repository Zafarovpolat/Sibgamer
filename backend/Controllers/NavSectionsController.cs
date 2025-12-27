using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers;

[ApiController]
[Route("api/nav-sections")]
public class NavSectionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NavSectionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить все опубликованные разделы навигации для меню
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetNavSections()
    {
        var sections = await _context.NavSections
            .Where(s => s.IsPublished)
            .OrderBy(s => s.Order)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Icon,
                s.Order,
                Type = s.Type.ToString().ToLower(),
                s.Url,
                s.IsExternal,
                s.OpenInNewTab,
                Items = s.Items
                    .Where(i => i.IsPublished)
                    .OrderBy(i => i.Order)
                    .Select(i => new
                    {
                        i.Id,
                        i.Name,
                        i.Icon,
                        i.Order,
                        Type = i.Type.ToString().ToLower(),
                        Url = i.Type == Models.NavItemType.CustomPage && i.CustomPage != null
                            ? $"/page/{i.CustomPage.Slug}"
                            : i.Url,
                        i.OpenInNewTab,
                        CustomPageSlug = i.CustomPage != null ? i.CustomPage.Slug : null
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(sections);
    }
}