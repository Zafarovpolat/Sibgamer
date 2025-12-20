using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Utils;

namespace backend.Models;

[Table("smtp_settings")]
public class SmtpSettings
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("host")]
    [MaxLength(255)]
    public string Host { get; set; } = string.Empty;

    [Required]
    [Column("port")]
    public int Port { get; set; }

    [Required]
    [Column("username")]
    [MaxLength(255)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    [MaxLength(500)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("enable_ssl")]
    public bool EnableSsl { get; set; } = true;

    [Required]
    [Column("from_email")]
    [MaxLength(255)]
    public string FromEmail { get; set; } = string.Empty;

    [Required]
    [Column("from_name")]
    [MaxLength(255)]
    public string FromName { get; set; } = string.Empty;

    [Column("is_configured")]
    public bool IsConfigured { get; set; } = false;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
