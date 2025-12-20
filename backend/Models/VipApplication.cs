using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Utils;

namespace backend.Models;

[Table("vip_applications")]
public class VipApplication
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    [StringLength(150)]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Column("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server? Server { get; set; }

    [Column("hours_per_week")]
    public int? HoursPerWeek { get; set; }

    [Required]
    [Column("reason")]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = "pending"; 

    [Column("admin_id")]
    public int? AdminId { get; set; }

    [Column("admin_comment")]
    public string? AdminComment { get; set; }

    [Column("vip_group")]
    [StringLength(128)]
    public string? VipGroup { get; set; }

    [Column("duration_days")]
    public int? DurationDays { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }
}
