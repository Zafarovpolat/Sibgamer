using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class News
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Summary { get; set; }

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    public string? CoverImage { get; set; }

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public int ViewCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;

    public ICollection<NewsMedia> Media { get; set; } = new List<NewsMedia>();
    public ICollection<NewsComment> Comments { get; set; } = new List<NewsComment>();
    public ICollection<NewsLike> Likes { get; set; } = new List<NewsLike>();
}

public class NewsMedia
{
    public int Id { get; set; }
    public int NewsId { get; set; }
    public News News { get; set; } = null!;

    [Required]
    public string MediaUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MediaType { get; set; } = string.Empty;

    public int Order { get; set; }
}

public class NewsComment
{
    public int Id { get; set; }

    public int NewsId { get; set; }
    public News News { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    public int? ParentCommentId { get; set; }
    public NewsComment? ParentComment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public ICollection<NewsComment> Replies { get; set; } = new List<NewsComment>();
}

public class NewsLike
{
    public int Id { get; set; }

    public int NewsId { get; set; }
    public News News { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
