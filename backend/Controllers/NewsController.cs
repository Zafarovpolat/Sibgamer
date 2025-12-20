using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Utils;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileService;
    private readonly IViewTrackingService _viewTrackingService;
    private readonly ITelegramService _telegramService;

    public NewsController(ApplicationDbContext context, IFileService fileService, IViewTrackingService viewTrackingService, ITelegramService telegramService)
    {
        _context = context;
        _fileService = fileService;
        _viewTrackingService = viewTrackingService;
        _telegramService = telegramService;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }

    [HttpGet]
    public async Task<ActionResult> GetNews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.News
            .Include(n => n.Author)
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var newsItems = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var news = newsItems.Select(n => new NewsListDto
        {
            Id = n.Id,
            Title = n.Title,
            Summary = n.Summary,
            Slug = n.Slug,
            CoverImage = n.CoverImage,
            AuthorName = n.Author.Username,
            CreatedAt = n.CreatedAt,
            ViewCount = n.ViewCount,
            LikeCount = n.LikeCount,
            CommentCount = _context.NewsComments.Count(c => c.NewsId == n.Id),
            IsPublished = n.IsPublished
        }).ToList();

        return Ok(new
        {
            items = news,
            totalCount,
            totalPages,
            currentPage = page,
            pageSize
        });
    }

    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<NewsListDto>>> GetLatestNews(int count = 3)
    {
        var newsItems = await _context.News
            .Include(n => n.Author)
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();

        var news = newsItems.Select(n => new NewsListDto
        {
            Id = n.Id,
            Title = n.Title,
            Summary = n.Summary,
            Slug = n.Slug,
            CoverImage = n.CoverImage,
            AuthorName = n.Author.Username,
            CreatedAt = n.CreatedAt,
            ViewCount = n.ViewCount,
            LikeCount = n.LikeCount,
            CommentCount = _context.NewsComments.Count(c => c.NewsId == n.Id),
            IsPublished = n.IsPublished
        }).ToList();

        return Ok(news);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<NewsDto>> GetNewsBySlug(string slug)
    {
        var currentUserId = GetCurrentUserId();

        var news = await _context.News
            .Include(n => n.Author)
            .Include(n => n.Media)
            .FirstOrDefaultAsync(n => n.Slug == slug && n.IsPublished);

        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        var ipAddress = IpAddressHelper.GetClientIpAddress(HttpContext);
        await _viewTrackingService.TrackNewsViewAsync(news.Id, currentUserId, ipAddress);

        var isLiked = false;
        if (currentUserId.HasValue)
        {
            isLiked = await _context.NewsLikes
                .AnyAsync(l => l.NewsId == news.Id && l.UserId == currentUserId.Value);
        }

        var result = new NewsDto
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            Summary = news.Summary,
            Slug = news.Slug,
            CoverImage = news.CoverImage,
            Author = new UserDto
            {
                Id = news.Author.Id,
                Username = news.Author.Username,
                Email = news.Author.Email,
                AvatarUrl = news.Author.AvatarUrl,
                IsAdmin = news.Author.IsAdmin,
                CreatedAt = news.Author.CreatedAt
            },
            IsPublished = news.IsPublished,
            CreatedAt = news.CreatedAt,
            UpdatedAt = news.UpdatedAt,
            ViewCount = news.ViewCount,
            LikeCount = news.LikeCount,
            CommentCount = news.CommentCount,
            Media = news.Media.OrderBy(m => m.Order).Select(m => new NewsMediaDto
            {
                Id = m.Id,
                MediaUrl = m.MediaUrl,
                MediaType = m.MediaType,
                Order = m.Order
            }).ToList(),
            IsLikedByCurrentUser = isLiked
        };

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<NewsDto>> CreateNews(CreateNewsDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var slug = string.IsNullOrWhiteSpace(dto.Slug) 
            ? SlugGenerator.Generate(dto.Title) 
            : SlugGenerator.Generate(dto.Slug);

        if (await _context.News.AnyAsync(n => n.Slug == slug))
        {
            slug = $"{slug}-{DateTimeHelper.GetServerLocalTime().Ticks}";
        }

        var news = new News
        {
            Title = dto.Title,
            Content = dto.Content,
            Summary = dto.Summary,
            Slug = slug,
            CoverImage = dto.CoverImage,
            AuthorId = userId.Value,
            IsPublished = dto.IsPublished,
            CreatedAt = DateTimeHelper.GetServerLocalTime()
        };

        if (dto.MediaUrls != null && dto.MediaUrls.Any())
        {
            int order = 0;
            foreach (var mediaUrl in dto.MediaUrls)
            {
                var mediaType = _fileService.GetMimeType(mediaUrl);

                news.Media.Add(new NewsMedia
                {
                    MediaUrl = mediaUrl,
                    MediaType = mediaType,
                    Order = order++
                });
            }
        }

        _context.News.Add(news);
        await _context.SaveChangesAsync();

        if (news.IsPublished)
        {
            try
            {
                await _telegramService.SendNewsNotificationAsync(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Telegram notification for news {news.Id}: {ex.Message}");
            }
        }

        return CreatedAtAction(nameof(GetNewsBySlug), new { slug = news.Slug }, await GetNewsBySlug(news.Slug));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<NewsDto>> UpdateNews(int id, UpdateNewsDto dto)
    {
        var news = await _context.News
            .Include(n => n.Media)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        var wasPublished = news.IsPublished;

        if (dto.Title != null)
            news.Title = dto.Title;

        if (dto.Content != null)
            news.Content = dto.Content;

        if (dto.Summary != null)
            news.Summary = dto.Summary;

        var slugSource = !string.IsNullOrWhiteSpace(dto.Slug) ? dto.Slug : dto.Title;
        if (!string.IsNullOrWhiteSpace(slugSource))
        {
            var newSlug = SlugGenerator.Generate(slugSource);
            if (newSlug != news.Slug)
            {
                if (await _context.News.AnyAsync(n => n.Slug == newSlug && n.Id != id))
                {
                    return BadRequest(new { message = "Эта ссылка уже используется" });
                }
                news.Slug = newSlug;
            }
        }

        if (dto.CoverImage != null)
        {
            if (!string.IsNullOrEmpty(news.CoverImage) && news.CoverImage != dto.CoverImage)
            {
                await _fileService.DeleteFileAsync(news.CoverImage);
            }
            news.CoverImage = dto.CoverImage;
        }

        if (dto.IsPublished.HasValue)
            news.IsPublished = dto.IsPublished.Value;

        if (dto.MediaUrls != null)
        {
            foreach (var oldMedia in news.Media)
            {
                await _fileService.DeleteFileAsync(oldMedia.MediaUrl);
            }

            _context.NewsMedia.RemoveRange(news.Media);

            int order = 0;
            foreach (var mediaUrl in dto.MediaUrls)
            {
                var mediaType = _fileService.GetMimeType(mediaUrl);

                news.Media.Add(new NewsMedia
                {
                    MediaUrl = mediaUrl,
                    MediaType = mediaType,
                    Order = order++
                });
            }
        }

        news.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        await _context.SaveChangesAsync();

        if (dto.IsPublished.HasValue && dto.IsPublished.Value && !wasPublished)
        {
            try
            {
                await _telegramService.SendNewsNotificationAsync(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send Telegram notification for news {news.Id}: {ex.Message}");
            }
        }

        return Ok(await GetNewsBySlug(news.Slug));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        var news = await _context.News
            .Include(n => n.Media)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        if (!string.IsNullOrEmpty(news.CoverImage))
        {
            await _fileService.DeleteFileAsync(news.CoverImage);
        }

        foreach (var media in news.Media)
        {
            await _fileService.DeleteFileAsync(media.MediaUrl);
        }

        _context.News.Remove(news);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Новость удалена" });
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> ToggleLike(int id)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var news = await _context.News.FindAsync(id);
        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        var existingLike = await _context.NewsLikes
            .FirstOrDefaultAsync(l => l.NewsId == id && l.UserId == userId.Value);

        if (existingLike != null)
        {
            _context.NewsLikes.Remove(existingLike);
            news.LikeCount--;
            await _context.SaveChangesAsync();
            return Ok(new { liked = false, likeCount = news.LikeCount });
        }
        else
        {
            _context.NewsLikes.Add(new NewsLike
            {
                NewsId = id,
                UserId = userId.Value,
                CreatedAt = DateTimeHelper.GetServerLocalTime()
            });
            news.LikeCount++;
            await _context.SaveChangesAsync();
            return Ok(new { liked = true, likeCount = news.LikeCount });
        }
    }

    [HttpGet("{id}/comments")]
    public async Task<ActionResult<IEnumerable<NewsCommentDto>>> GetComments(int id)
    {
        var allComments = await _context.NewsComments
            .Include(c => c.User)
            .Where(c => c.NewsId == id)
            .OrderByDescending(c => c.CreatedAt) 
            .AsNoTracking()
            .ToListAsync();

        var commentDict = allComments.ToDictionary(c => c.Id);
        var result = allComments.Select(c => MapCommentToDtoFlat(c, commentDict)).ToList();
        return Ok(result);
    }

    private NewsCommentDto MapCommentToDtoFlat(NewsComment comment, Dictionary<int, NewsComment> commentDict)
    {
        ParentCommentInfo? parentInfo = null;
        if (comment.ParentCommentId.HasValue && commentDict.ContainsKey(comment.ParentCommentId.Value))
        {
            var parent = commentDict[comment.ParentCommentId.Value];
            parentInfo = new ParentCommentInfo
            {
                Id = parent.Id,
                Username = parent.User.Username,
                ContentPreview = parent.Content.Length > 50 ? parent.Content.Substring(0, 50) + "..." : parent.Content
            };
        }

        return new NewsCommentDto
        {
            Id = comment.Id,
            NewsId = comment.NewsId,
            User = new UserDto
            {
                Id = comment.User.Id,
                Username = comment.User.Username,
                Email = comment.User.Email,
                AvatarUrl = comment.User.AvatarUrl,
                IsAdmin = comment.User.IsAdmin,
                CreatedAt = comment.User.CreatedAt
            },
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            ParentComment = parentInfo,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Replies = new List<NewsCommentDto>() 
        };
    }

    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<NewsCommentDto>> CreateComment(int id, CreateCommentDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var news = await _context.News.FindAsync(id);
        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        if (dto.ParentCommentId.HasValue)
        {
            var parentComment = await _context.NewsComments.FindAsync(dto.ParentCommentId.Value);
            if (parentComment == null || parentComment.NewsId != id)
                return BadRequest(new { message = "Родительский комментарий не найден" });
        }

        var comment = new NewsComment
        {
            NewsId = id,
            UserId = userId.Value,
            Content = dto.Content,
            ParentCommentId = dto.ParentCommentId,
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.NewsComments.Add(comment);
        news.CommentCount++;
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId.Value);
        var result = new NewsCommentDto
        {
            Id = comment.Id,
            NewsId = comment.NewsId,
            User = new UserDto
            {
                Id = user!.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt
            },
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Replies = new List<NewsCommentDto>()
        };

        return CreatedAtAction(nameof(GetComments), new { id }, result);
    }

    [Authorize]
    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var comment = await _context.NewsComments
            .Include(c => c.News)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null)
            return NotFound(new { message = "Комментарий не найден" });

        var user = await _context.Users.FindAsync(userId.Value);
        
        if (comment.UserId != userId.Value && !user!.IsAdmin)
            return Forbid();

        var commentCount = await CountCommentsRecursive(commentId);

        _context.NewsComments.Remove(comment);
        comment.News.CommentCount -= commentCount;
        
        if (comment.News.CommentCount < 0)
            comment.News.CommentCount = 0;
            
        await _context.SaveChangesAsync();

        return Ok(new { message = "Комментарий удален" });
    }

    private async Task<int> CountCommentsRecursive(int commentId)
    {
        var replies = await _context.NewsComments
            .Where(c => c.ParentCommentId == commentId)
            .ToListAsync();

        int count = 1;
        foreach (var reply in replies)
        {
            count += await CountCommentsRecursive(reply.Id);
        }

        return count;
    }
}
