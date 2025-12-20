using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomPagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IViewTrackingService _viewTrackingService;

    public CustomPagesController(ApplicationDbContext context, IViewTrackingService viewTrackingService)
    {
        _context = context;
        _viewTrackingService = viewTrackingService;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }

    [HttpGet]
    public async Task<ActionResult> GetCustomPages([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.CustomPages
            .Include(p => p.Author)
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pages = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var customPages = pages.Select(p => new CustomPageListDto
        {
            Id = p.Id,
            Title = p.Title,
            Summary = p.Summary,
            Slug = p.Slug,
            CoverImage = p.CoverImage,
            AuthorName = p.Author.Username,
            CreatedAt = p.CreatedAt,
            ViewCount = p.ViewCount,
            IsPublished = p.IsPublished
        }).ToList();

        return Ok(new
        {
            items = customPages,
            totalCount,
            totalPages,
            currentPage = page,
            pageSize
        });
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<CustomPageDto>> GetCustomPageBySlug(string slug)
    {
        var currentUserId = GetCurrentUserId();

        var page = await _context.CustomPages
            .Include(p => p.Author)
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

        if (page == null)
            return NotFound(new { message = "Страница не найдена" });

        var ipAddress = IpAddressHelper.GetClientIpAddress(HttpContext);
        await _viewTrackingService.TrackCustomPageViewAsync(page.Id, currentUserId, ipAddress);

        var result = new CustomPageDto
        {
            Id = page.Id,
            Title = page.Title,
            Content = page.Content,
            Summary = page.Summary,
            Slug = page.Slug,
            CoverImage = page.CoverImage,
            Author = new UserDto
            {
                Id = page.Author.Id,
                Username = page.Author.Username,
                Email = page.Author.Email,
                AvatarUrl = page.Author.AvatarUrl,
                IsAdmin = page.Author.IsAdmin,
                CreatedAt = page.Author.CreatedAt
            },
            IsPublished = page.IsPublished,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            ViewCount = page.ViewCount,
            Media = page.Media.OrderBy(m => m.Order).Select(m => new CustomPageMediaDto
            {
                Id = m.Id,
                MediaUrl = m.MediaUrl,
                MediaType = m.MediaType,
                Order = m.Order
            }).ToList()
        };

        return Ok(result);
    }
}
