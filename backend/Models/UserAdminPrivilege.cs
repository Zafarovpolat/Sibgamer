using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Utils;

namespace backend.Models;
[Table("user_admin_privileges")]
public class UserAdminPrivilege
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
    [StringLength(50)]
    [Column("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server? Server { get; set; }

    [Column("tariff_id")]
    public int? TariffId { get; set; }

    [ForeignKey("TariffId")]
    public AdminTariff? Tariff { get; set; }

    [Column("tariff_option_id")]
    public int? TariffOptionId { get; set; }

    [ForeignKey("TariffOptionId")]
    public AdminTariffOption? TariffOption { get; set; }

    [Column("flags")]
    [StringLength(100)]
    public string? Flags { get; set; }

    [Column("group_name")]
    [StringLength(100)]
    public string? GroupName { get; set; }

    [Column("immunity")]
    public int Immunity { get; set; } = 0;

    [Column("starts_at")]
    public DateTime StartsAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("sourcebans_admin_id")]
    public int? SourceBansAdminId { get; set; }

    [Column("transaction_id")]
    public int? TransactionId { get; set; }

    public DonationTransaction? Transaction { get; set; }

    [Column("admin_password")]
    [StringLength(100)]
    public string? AdminPassword { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}

[Table("user_vip_privileges")]
public class UserVipPrivilege
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
    [StringLength(50)]
    [Column("steam_id")]
    public string SteamId { get; set; } = string.Empty;

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server? Server { get; set; }

    [Column("tariff_id")]
    public int? TariffId { get; set; }

    [ForeignKey("TariffId")]
    public VipTariff? Tariff { get; set; }

    [Column("tariff_option_id")]
    public int? TariffOptionId { get; set; }

    [ForeignKey("TariffOptionId")]
    public VipTariffOption? TariffOption { get; set; }

    [Column("group_name")]
    [StringLength(64)]
    public string GroupName { get; set; } = string.Empty;

    [Column("starts_at")]
    public DateTime StartsAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("transaction_id")]
    public int? TransactionId { get; set; }

    public DonationTransaction? Transaction { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
