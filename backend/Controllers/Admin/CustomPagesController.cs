using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Utils;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class CustomPagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;

    public CustomPagesController(ApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult> GetCustomPages([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.CustomPages
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pages = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Summary,
                p.Slug,
                p.CoverImage,
                p.IsPublished,
                p.ViewCount,
                p.CreatedAt,
                Author = new { p.Author.Username }
            })
            .ToListAsync();

        return Ok(new
        {
            items = pages,
            totalCount,
            totalPages,
            currentPage = page,
            pageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomPage>> GetCustomPage(int id)
    {
        var page = await _context.CustomPages
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            return NotFound(new { message = "Страница не найдена" });
        }

        return Ok(page);
    }

    [HttpPost]
    public async Task<ActionResult<CustomPage>> CreateCustomPage([FromBody] CreateCustomPageDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Пользователь не авторизован" });
        }

        var slug = !string.IsNullOrWhiteSpace(dto.Slug)
            ? SlugGenerator.Generate(dto.Slug)
            : SlugGenerator.Generate(dto.Title);

        var existingPage = await _context.CustomPages.FirstOrDefaultAsync(p => p.Slug == slug);
        if (existingPage != null)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        var page = new CustomPage
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            Slug = slug,
            CoverImage = dto.CoverImage,
            AuthorId = userId,
            IsPublished = dto.IsPublished,
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        if (dto.MediaUrls != null && dto.MediaUrls.Any())
        {
            int order = 0;
            foreach (var mediaUrl in dto.MediaUrls)
            {
                var mediaType = _fileService.GetMimeType(mediaUrl);

                page.Media.Add(new CustomPageMedia
                {
                    MediaUrl = mediaUrl,
                    MediaType = mediaType,
                    Order = order++
                });
            }
        }

        _context.CustomPages.Add(page);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomPage), new { id = page.Id }, page);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomPage(int id, [FromBody] UpdateCustomPageDto dto)
    {
        var page = await _context.CustomPages.FindAsync(id);

        if (page == null)
        {
            return NotFound(new { message = "Страница не найдена" });
        }

        if (!string.IsNullOrEmpty(page.CoverImage) &&
            !string.IsNullOrEmpty(dto.CoverImage) &&
            page.CoverImage != dto.CoverImage)
        {
            await _fileService.DeleteFileAsync(page.CoverImage);
        }

        page.Title = dto.Title;
        page.Content = dto.Content;
        page.Summary = dto.Summary;
        page.CoverImage = dto.CoverImage;
        page.IsPublished = dto.IsPublished;
        page.UpdatedAt = DateTimeHelper.GetServerLocalTime();

        var newSlug = !string.IsNullOrWhiteSpace(dto.Slug)
            ? SlugGenerator.Generate(dto.Slug)
            : SlugGenerator.Generate(dto.Title);

        if (page.Slug != newSlug)
        {
            var existingPage = await _context.CustomPages.FirstOrDefaultAsync(p => p.Slug == newSlug && p.Id != id);
            if (existingPage != null)
            {
                return BadRequest(new { message = "Эта ссылка уже используется" });
            }
            page.Slug = newSlug;
        }

        if (dto.MediaUrls != null)
        {
            foreach (var oldMedia in page.Media)
            {
                await _fileService.DeleteFileAsync(oldMedia.MediaUrl);
            }

            _context.CustomPageMedia.RemoveRange(page.Media);

            int order = 0;
            foreach (var mediaUrl in dto.MediaUrls)
            {
                var mediaType = _fileService.GetMimeType(mediaUrl);

                page.Media.Add(new CustomPageMedia
                {
                    MediaUrl = mediaUrl,
                    MediaType = mediaType,
                    Order = order++
                });
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CustomPageExists(id))
            {
                return NotFound(new { message = "Страница не найдена" });
            }
            throw;
        }

        return Ok(page);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomPage(int id)
    {
        var page = await _context.CustomPages
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            return NotFound(new { message = "Страница не найдена" });
        }

        if (!string.IsNullOrEmpty(page.CoverImage))
        {
            await _fileService.DeleteFileAsync(page.CoverImage);
        }

        foreach (var media in page.Media)
        {
            await _fileService.DeleteFileAsync(media.MediaUrl);
        }

        _context.CustomPages.Remove(page);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CustomPageExists(int id)
    {
        return await _context.CustomPages.AnyAsync(p => p.Id == id);
    }
}

public class CreateCustomPageDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public bool IsPublished { get; set; } = true;
    public List<string>? MediaUrls { get; set; }
}

public class UpdateCustomPageDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public bool IsPublished { get; set; }
    public List<string>? MediaUrls { get; set; }
}
