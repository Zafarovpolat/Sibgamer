using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class Event
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Summary { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;
    
    public string? CoverImage { get; set; }
    
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    
    public bool IsPublished { get; set; } = true;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    
    public bool IsStartNotificationSent { get; set; } = false;
    public bool IsEndNotificationSent { get; set; } = false;
    public bool IsCreatedNotificationSent { get; set; } = false;
    
    public ICollection<EventComment> Comments { get; set; } = new List<EventComment>();
    public ICollection<EventLike> Likes { get; set; } = new List<EventLike>();
    public ICollection<EventMedia> Media { get; set; } = new List<EventMedia>();
}

public class EventMedia
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public string MediaUrl { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string MediaType { get; set; } = "image";
    public int Order { get; set; }
}

public class EventComment
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    public int? ParentCommentId { get; set; }
    public EventComment? ParentComment { get; set; }
    public ICollection<EventComment> Replies { get; set; } = new List<EventComment>();
    
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}

public class EventLike
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
