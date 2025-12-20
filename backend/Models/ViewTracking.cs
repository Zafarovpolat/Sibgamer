using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class NewsView
{
    public int Id { get; set; }

    public int NewsId { get; set; }
    public News News { get; set; } = null!;

    public int? UserId { get; set; } 
    public User? User { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; } 

    public DateTime ViewDate { get; set; } = DateTimeHelper.GetServerLocalTime();
}

public class EventView
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int? UserId { get; set; } 
    public User? User { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

    public DateTime ViewDate { get; set; } = DateTimeHelper.GetServerLocalTime();
}

public class CustomPageView
{
    public int Id { get; set; }

    public int CustomPageId { get; set; }
    public CustomPage CustomPage { get; set; } = null!;

    public int? UserId { get; set; } 
    public User? User { get; set; }

    [StringLength(45)]
    public string? IpAddress { get; set; }

    public DateTime ViewDate { get; set; } = DateTimeHelper.GetServerLocalTime();
}
