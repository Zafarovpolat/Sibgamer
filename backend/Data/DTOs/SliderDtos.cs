using System.ComponentModel.DataAnnotations;

namespace backend.Data.DTOs;

public class CreateSliderImageDto
{
    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Order { get; set; } = 0;

    public List<ButtonDto>? Buttons { get; set; }
}

public class UpdateSliderImageDto
{
    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Order { get; set; } = 0;

    public List<ButtonDto>? Buttons { get; set; }
}

public class ButtonDto
{
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
