using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    [StringLength(50)]
    public string? SteamId { get; set; } 

    public string? SteamProfileUrl { get; set; }

    public string? LastIp { get; set; }

    public bool IsAdmin { get; set; } = false;

    public bool IsBlocked { get; set; } = false;

    public DateTime? BlockedAt { get; set; }

    public string? BlockReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
