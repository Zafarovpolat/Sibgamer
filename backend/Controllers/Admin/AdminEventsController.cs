using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Claims;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly ITelegramService _telegramService;

    public EventsController(ApplicationDbContext context, IFileService fileService, ITelegramService telegramService)
    {
        _context = context;
        _fileService = fileService;
        _telegramService = telegramService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetEvents()
    {
        var events = await _context.Events
            .Include(e => e.Author)
            .OrderByDescending(e => e.StartDate)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Summary,
                e.Slug,
                e.CoverImage,
                e.StartDate,
                e.EndDate,
                e.IsPublished,
                e.ViewCount,
                e.LikeCount,
                e.CommentCount,
                e.CreatedAt,
                Author = new { e.Author.Username }
            })
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var eventItem = await _context.Events
            .Include(e => e.Media)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        return Ok(eventItem);
    }

    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Пользователь не авторизован" });
        }

        var slug = !string.IsNullOrWhiteSpace(dto.Slug) 
            ? SlugGenerator.Generate(dto.Slug) 
            : SlugGenerator.Generate(dto.Title);

        var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Slug == slug);
        if (existingEvent != null)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

            var eventItem = new Event
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            Slug = slug,
            CoverImage = dto.CoverImage,
            AuthorId = userId,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Local),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Local),
            IsPublished = dto.IsPublished,
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();

        if (eventItem.IsPublished)
        {
            try
            {
                await _telegramService.SendEventCreatedNotificationAsync(eventItem);
                eventItem.IsCreatedNotificationSent = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Telegram notification: {ex.Message}");
            }
            try
            {
                var now = DateTimeHelper.GetServerLocalTime();
                if (!eventItem.IsStartNotificationSent && eventItem.StartDate <= now)
                {
                    await _telegramService.SendEventStartedNotificationAsync(eventItem);
                    eventItem.IsStartNotificationSent = true;
                    await _context.SaveChangesAsync();
                }

                if (!eventItem.IsEndNotificationSent && eventItem.EndDate <= now)
                {
                    await _telegramService.SendEventEndedNotificationAsync(eventItem);
                    eventItem.IsEndNotificationSent = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send start/end Telegram notifications: {ex.Message}");
            }
        }

        return CreatedAtAction(nameof(GetEvent), new { id = eventItem.Id }, eventItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto dto)
    {
        var eventItem = await _context.Events.FindAsync(id);

        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        if (!string.IsNullOrEmpty(eventItem.CoverImage) && 
            !string.IsNullOrEmpty(dto.CoverImage) && 
            eventItem.CoverImage != dto.CoverImage)
        {
            await _fileService.DeleteFileAsync(eventItem.CoverImage);
        }

        eventItem.Title = dto.Title;
        eventItem.Content = dto.Content;
        eventItem.Summary = dto.Summary;
        eventItem.CoverImage = dto.CoverImage;
        eventItem.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Local);
        eventItem.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Local);
        eventItem.IsPublished = dto.IsPublished;
        eventItem.UpdatedAt = DateTimeHelper.GetServerLocalTime();

        var newSlug = !string.IsNullOrWhiteSpace(dto.Slug) 
            ? SlugGenerator.Generate(dto.Slug) 
            : SlugGenerator.Generate(dto.Title);
            
        if (eventItem.Slug != newSlug)
        {
            var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Slug == newSlug && e.Id != id);
            if (existingEvent != null)
            {
                newSlug = $"{newSlug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }
            eventItem.Slug = newSlug;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EventExists(id))
            {
                return NotFound(new { message = "Событие не найдено" });
            }
            throw;
        }

        return Ok(eventItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventItem = await _context.Events
            .Include(e => e.Media)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        if (!string.IsNullOrEmpty(eventItem.CoverImage))
        {
            await _fileService.DeleteFileAsync(eventItem.CoverImage);
        }

        foreach (var media in eventItem.Media)
        {
            await _fileService.DeleteFileAsync(media.MediaUrl);
        }

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> EventExists(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }
}

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublished { get; set; } = true;
}

public class UpdateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublished { get; set; }
}
