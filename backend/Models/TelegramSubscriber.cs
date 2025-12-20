using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class TelegramSubscriber
{
    public int Id { get; set; }

    [Required]
    public long ChatId { get; set; } 

    public int? UserId { get; set; } 

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime SubscribedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public User? User { get; set; }
}
