using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Utils;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/nav-sections")]
[Authorize(Roles = "Admin")]
public class AdminNavSectionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminNavSectionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==================== SECTIONS ====================

    /// <summary>
    /// Получить все разделы
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sections = await _context.NavSections
            .OrderBy(s => s.Order)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Icon,
                s.Order,
                s.IsPublished,
                Type = s.Type.ToString().ToLower(),
                s.Url,
                s.IsExternal,
                s.OpenInNewTab,
                s.CreatedAt,
                s.UpdatedAt,
                ItemsCount = s.Items.Count,
                Items = s.Items
                    .OrderBy(i => i.Order)
                    .Select(i => new
                    {
                        i.Id,
                        i.Name,
                        i.Icon,
                        i.Order,
                        i.IsPublished,
                        Type = i.Type.ToString().ToLower(),
                        i.Url,
                        i.CustomPageId,
                        CustomPageTitle = i.CustomPage != null ? i.CustomPage.Title : null,
                        CustomPageSlug = i.CustomPage != null ? i.CustomPage.Slug : null,
                        i.OpenInNewTab,
                        i.CreatedAt
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(sections);
    }

    /// <summary>
    /// Получить раздел по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var section = await _context.NavSections
            .Include(s => s.Items.OrderBy(i => i.Order))
                .ThenInclude(i => i.CustomPage)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section == null)
            return NotFound(new { message = "Раздел не найден" });

        return Ok(new
        {
            section.Id,
            section.Name,
            section.Icon,
            section.Order,
            section.IsPublished,
            Type = section.Type.ToString().ToLower(),
            section.Url,
            section.IsExternal,
            section.OpenInNewTab,
            section.CreatedAt,
            section.UpdatedAt,
            Items = section.Items.Select(i => new
            {
                i.Id,
                i.Name,
                i.Icon,
                i.Order,
                i.IsPublished,
                Type = i.Type.ToString().ToLower(),
                i.Url,
                i.CustomPageId,
                CustomPageTitle = i.CustomPage?.Title,
                CustomPageSlug = i.CustomPage?.Slug,
                i.OpenInNewTab
            })
        });
    }

    /// <summary>
    /// Создать раздел
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNavSectionDto dto)
    {
        var maxOrder = await _context.NavSections.MaxAsync(s => (int?)s.Order) ?? 0;

        var section = new NavSection
        {
            Name = dto.Name,
            Icon = dto.Icon,
            Order = dto.Order ?? maxOrder + 1,
            IsPublished = dto.IsPublished,
            Type = Enum.Parse<NavSectionType>(dto.Type, true),
            Url = dto.Url,
            IsExternal = dto.IsExternal,
            OpenInNewTab = dto.OpenInNewTab
        };

        _context.NavSections.Add(section);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Раздел создан", id = section.Id });
    }

    /// <summary>
    /// Обновить раздел
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNavSectionDto dto)
    {
        var section = await _context.NavSections.FindAsync(id);
        if (section == null)
            return NotFound(new { message = "Раздел не найден" });

        section.Name = dto.Name;
        section.Icon = dto.Icon;
        section.Order = dto.Order;
        section.IsPublished = dto.IsPublished;
        section.Type = Enum.Parse<NavSectionType>(dto.Type, true);
        section.Url = dto.Url;
        section.IsExternal = dto.IsExternal;
        section.OpenInNewTab = dto.OpenInNewTab;
        section.UpdatedAt = DateTimeHelper.GetServerLocalTime();

        await _context.SaveChangesAsync();

        return Ok(new { message = "Раздел обновлён" });
    }

    /// <summary>
    /// Удалить раздел
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var section = await _context.NavSections
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section == null)
            return NotFound(new { message = "Раздел не найден" });

        _context.NavSections.Remove(section);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Раздел удалён" });
    }

    /// <summary>
    /// Обновить порядок разделов
    /// </summary>
    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder([FromBody] List<ReorderDto> items)
    {
        foreach (var item in items)
        {
            var section = await _context.NavSections.FindAsync(item.Id);
            if (section != null)
            {
                section.Order = item.Order;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Порядок обновлён" });
    }

    // ==================== ITEMS ====================

    /// <summary>
    /// Добавить пункт в раздел
    /// </summary>
    [HttpPost("{sectionId}/items")]
    public async Task<IActionResult> AddItem(int sectionId, [FromBody] CreateNavItemDto dto)
    {
        var section = await _context.NavSections.FindAsync(sectionId);
        if (section == null)
            return NotFound(new { message = "Раздел не найден" });

        var maxOrder = await _context.NavSectionItems
            .Where(i => i.SectionId == sectionId)
            .MaxAsync(i => (int?)i.Order) ?? 0;

        var item = new NavSectionItem
        {
            SectionId = sectionId,
            Name = dto.Name,
            Icon = dto.Icon,
            Order = dto.Order ?? maxOrder + 1,
            IsPublished = dto.IsPublished,
            Type = Enum.Parse<NavItemType>(dto.Type, true),
            Url = dto.Url,
            CustomPageId = dto.CustomPageId,
            OpenInNewTab = dto.OpenInNewTab
        };

        _context.NavSectionItems.Add(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Пункт добавлен", id = item.Id });
    }

    /// <summary>
    /// Обновить пункт
    /// </summary>
    [HttpPut("items/{itemId}")]
    public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateNavItemDto dto)
    {
        var item = await _context.NavSectionItems.FindAsync(itemId);
        if (item == null)
            return NotFound(new { message = "Пункт не найден" });

        item.Name = dto.Name;
        item.Icon = dto.Icon;
        item.Order = dto.Order;
        item.IsPublished = dto.IsPublished;
        item.Type = Enum.Parse<NavItemType>(dto.Type, true);
        item.Url = dto.Url;
        item.CustomPageId = dto.CustomPageId;
        item.OpenInNewTab = dto.OpenInNewTab;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Пункт обновлён" });
    }

    /// <summary>
    /// Удалить пункт
    /// </summary>
    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> DeleteItem(int itemId)
    {
        var item = await _context.NavSectionItems.FindAsync(itemId);
        if (item == null)
            return NotFound(new { message = "Пункт не найден" });

        _context.NavSectionItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Пункт удалён" });
    }

    /// <summary>
    /// Обновить порядок пунктов в разделе
    /// </summary>
    [HttpPut("{sectionId}/items/reorder")]
    public async Task<IActionResult> ReorderItems(int sectionId, [FromBody] List<ReorderDto> items)
    {
        foreach (var itemDto in items)
        {
            var item = await _context.NavSectionItems
                .FirstOrDefaultAsync(i => i.Id == itemDto.Id && i.SectionId == sectionId);

            if (item != null)
            {
                item.Order = itemDto.Order;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Порядок обновлён" });
    }

    /// <summary>
    /// Получить кастомные страницы для выбора
    /// </summary>
    [HttpGet("custom-pages")]
    public async Task<IActionResult> GetCustomPages()
    {
        var pages = await _context.CustomPages
            .Where(p => p.IsPublished)
            .OrderBy(p => p.Title)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Slug
            })
            .ToListAsync();

        return Ok(pages);
    }
}

// ==================== DTOs ====================

public class CreateNavSectionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public bool IsPublished { get; set; } = true;
    public string Type { get; set; } = "link"; // "link" или "dropdown"
    public string? Url { get; set; }
    public bool IsExternal { get; set; } = false;
    public bool OpenInNewTab { get; set; } = false;
}

public class UpdateNavSectionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; }
    public string Type { get; set; } = "link";
    public string? Url { get; set; }
    public bool IsExternal { get; set; }
    public bool OpenInNewTab { get; set; }
}

public class CreateNavItemDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public bool IsPublished { get; set; } = true;
    public string Type { get; set; } = "internallink"; // "internallink", "externallink", "custompage"
    public string? Url { get; set; }
    public int? CustomPageId { get; set; }
    public bool OpenInNewTab { get; set; } = false;
}

public class UpdateNavItemDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; }
    public string Type { get; set; } = "internallink";
    public string? Url { get; set; }
    public int? CustomPageId { get; set; }
    public bool OpenInNewTab { get; set; }
}

public class ReorderDto
{
    public int Id { get; set; }
    public int Order { get; set; }
}