using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class SliderImage
{
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Order { get; set; } = 0;

    public string? Buttons { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
