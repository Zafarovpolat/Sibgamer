using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class BlockedIp
{
    public int Id { get; set; }

    [Required]
    [StringLength(45)] 
    public string IpAddress { get; set; } = string.Empty;

    public string? Reason { get; set; }

    public DateTime BlockedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public int? BlockedByUserId { get; set; }
    public User? BlockedByUser { get; set; }
}
