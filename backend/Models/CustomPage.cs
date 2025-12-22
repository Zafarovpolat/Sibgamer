using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Utils;

namespace backend.Models;

public class CustomPage
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

    public ICollection<CustomPageMedia> Media { get; set; } = new List<CustomPageMedia>();
}

public class CustomPageMedia
{
    public int Id { get; set; }
    public int CustomPageId { get; set; }
    
    [JsonIgnore]  // <-- ДОБАВИТЬ ЭТУ СТРОКУ
    public CustomPage CustomPage { get; set; } = null!;

    [Required]
    public string MediaUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MediaType { get; set; } = string.Empty;

    public int Order { get; set; }
}