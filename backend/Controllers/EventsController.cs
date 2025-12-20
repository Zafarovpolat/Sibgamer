using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IViewTrackingService _viewTrackingService;

    public EventsController(ApplicationDbContext context, IViewTrackingService viewTrackingService)
    {
        _context = context;
        _viewTrackingService = viewTrackingService;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 6)
    {
        var query = _context.Events
            .Where(e => e.IsPublished)
            .Include(e => e.Author)
            .OrderByDescending(e => e.StartDate);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var events = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Summary,
                e.Slug,
                e.CoverImage,
                e.StartDate,
                e.EndDate,
                e.ViewCount,
                e.LikeCount,
                e.CommentCount,
                e.CreatedAt,
                Author = new { e.Author.Id, e.Author.Username, e.Author.AvatarUrl }
            })
            .ToListAsync();

        return Ok(new { items = events, totalCount, totalPages, currentPage = page });
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<object>> GetUpcomingEvents([FromQuery] int count = 3)
    {
        var now = DateTimeHelper.GetServerLocalTime();
        
        var events = await _context.Events
            .Where(e => e.IsPublished && e.EndDate > now)
            .Include(e => e.Author)
            .OrderBy(e => e.StartDate)
            .Take(count)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Summary,
                e.Slug,
                e.CoverImage,
                e.StartDate,
                e.EndDate,
                e.ViewCount,
                e.LikeCount,
                e.CommentCount,
                Author = new { e.Author.Id, e.Author.Username, e.Author.AvatarUrl }
            })
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<object>> GetEvent(string slug)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;

        var eventItem = await _context.Events
            .Include(e => e.Author)
            .Include(e => e.Media)
            .FirstOrDefaultAsync(e => e.Slug == slug && e.IsPublished);

        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        var ipAddress = IpAddressHelper.GetClientIpAddress(HttpContext);
        await _viewTrackingService.TrackEventViewAsync(eventItem.Id, userId, ipAddress);

        var isLiked = false;
        if (userId.HasValue)
        {
            isLiked = await _context.EventLikes
                .AnyAsync(l => l.EventId == eventItem.Id && l.UserId == userId.Value);
        }

        var result = new
        {
            eventItem.Id,
            eventItem.Title,
            eventItem.Content,
            eventItem.Summary,
            eventItem.Slug,
            eventItem.CoverImage,
            eventItem.StartDate,
            eventItem.EndDate,
            eventItem.ViewCount,
            eventItem.LikeCount,
            eventItem.CommentCount,
            eventItem.CreatedAt,
            eventItem.UpdatedAt,
            IsPublished = eventItem.IsPublished,
            Author = new { 
                eventItem.Author.Id, 
                eventItem.Author.Username, 
                eventItem.Author.AvatarUrl,
                eventItem.Author.IsAdmin 
            },
            Media = eventItem.Media.OrderBy(m => m.Order).Select(m => new { m.Id, m.MediaUrl, m.MediaType }),
            IsLikedByCurrentUser = isLiked
        };

        return Ok(result);
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<IEnumerable<object>>> GetComments(int id)
    {
        var allComments = await _context.EventComments
            .Include(c => c.User)
            .Where(c => c.EventId == id)
            .OrderByDescending(c => c.CreatedAt)
            .AsNoTracking()
            .ToListAsync();

        var commentDict = allComments.ToDictionary(c => c.Id);
        var result = allComments.Select(c => MapCommentToDtoFlat(c, commentDict)).ToList();
        return Ok(result);
    }

    private object MapCommentToDtoFlat(EventComment comment, Dictionary<int, EventComment> commentDict)
    {
        object? parentInfo = null;
        if (comment.ParentCommentId.HasValue && commentDict.ContainsKey(comment.ParentCommentId.Value))
        {
            var parent = commentDict[comment.ParentCommentId.Value];
            parentInfo = new
            {
                Id = parent.Id,
                Username = parent.User.Username,
                ContentPreview = parent.Content.Length > 50 ? parent.Content.Substring(0, 50) + "..." : parent.Content
            };
        }

        return new
        {
            comment.Id,
            EventId = comment.EventId,
            User = new 
            { 
                comment.User.Id, 
                comment.User.Username, 
                comment.User.AvatarUrl,
                comment.User.IsAdmin
            },
            comment.Content,
            ParentCommentId = comment.ParentCommentId,
            ParentComment = parentInfo,
            comment.CreatedAt,
            comment.UpdatedAt
        };
    }

    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<object>> AddComment(int id, [FromBody] AddCommentDto dto)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Пользователь не авторизован" });
        }

        var comment = new EventComment
        {
            EventId = id,
            UserId = userId,
            Content = dto.Content,
            ParentCommentId = dto.ParentCommentId,
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.EventComments.Add(comment);
        eventItem.CommentCount++;
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        return Ok(new
        {
            comment.Id,
            EventId = comment.EventId,
            User = new 
            { 
                user!.Id, 
                user.Username, 
                user.AvatarUrl,
                user.IsAdmin
            },
            comment.Content,
            ParentCommentId = comment.ParentCommentId,
            comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Replies = new List<object>() 
        });
    }

    [Authorize]
    [HttpDelete("comments/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.EventComments
            .Include(c => c.Event)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound(new { message = "Комментарий не найден" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Пользователь не авторизован" });
        }
        
        var user = await _context.Users.FindAsync(userId);
        var isAdmin = user?.IsAdmin ?? false;

        if (comment.UserId != userId && !isAdmin)
        {
            return Forbid();
        }

        var commentCount = await CountCommentsRecursive(id);

        _context.EventComments.Remove(comment);
        comment.Event.CommentCount -= commentCount;
        
        if (comment.Event.CommentCount < 0)
            comment.Event.CommentCount = 0;
            
        await _context.SaveChangesAsync();

        return Ok(new { message = "Комментарий удален" });
    }

    private async Task<int> CountCommentsRecursive(int commentId)
    {
        var replies = await _context.EventComments
            .Where(c => c.ParentCommentId == commentId)
            .ToListAsync();

        int count = 1;
        foreach (var reply in replies)
        {
            count += await CountCommentsRecursive(reply.Id);
        }

        return count;
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<ActionResult<object>> ToggleLike(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
        {
            return NotFound(new { message = "Событие не найдено" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Пользователь не авторизован" });
        }

        var existingLike = await _context.EventLikes
            .FirstOrDefaultAsync(l => l.EventId == id && l.UserId == userId);

        if (existingLike != null)
        {
            _context.EventLikes.Remove(existingLike);
            eventItem.LikeCount--;
            await _context.SaveChangesAsync();
            return Ok(new { liked = false, likeCount = eventItem.LikeCount });
        }
        else
        {
            var like = new EventLike
            {
                EventId = id,
                UserId = userId,
                CreatedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.EventLikes.Add(like);
            eventItem.LikeCount++;
            await _context.SaveChangesAsync();
            return Ok(new { liked = true, likeCount = eventItem.LikeCount });
        }
    }
}

public class AddCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}
