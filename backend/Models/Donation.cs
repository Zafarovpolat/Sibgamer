using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Utils;

namespace backend.Models;

[Table("yoomoney_settings")]
public class YooMoneySettings
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("wallet_number")]
    [MaxLength(100)]
    public string WalletNumber { get; set; } = string.Empty;

    [Required]
    [Column("secret_key")]
    [MaxLength(500)]
    public string SecretKey { get; set; } = string.Empty;

    [Column("is_configured")]
    public bool IsConfigured { get; set; } = false;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}


[Table("sourcebans_settings")]
public class SourceBansSettings
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server Server { get; set; } = null!;

    [Required]
    [Column("host")]
    [MaxLength(255)]
    public string Host { get; set; } = string.Empty;

    [Required]
    [Column("port")]
    public int Port { get; set; } = 3306;

    [Required]
    [Column("database")]
    [MaxLength(100)]
    public string Database { get; set; } = string.Empty;

    [Required]
    [Column("username")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    [MaxLength(500)]
    public string Password { get; set; } = string.Empty;

    [Column("is_configured")]
    public bool IsConfigured { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}

[Table("admin_tariff_groups")]
public class AdminTariffGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server Server { get; set; } = null!;

    [Required]
    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Column("order")]
    public int Order { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public ICollection<AdminTariff> Tariffs { get; set; } = new List<AdminTariff>();
}

[Table("admin_tariff_options")]
public class AdminTariffOption
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("tariff_id")]
    public int TariffId { get; set; }

    [ForeignKey("TariffId")]
    public AdminTariff Tariff { get; set; } = null!;

    [Required]
    [Column("duration_days")]
    public int DurationDays { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("order")]
    public int Order { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("requires_password")]
    public bool RequiresPassword { get; set; } = true;
}

[Table("admin_tariffs")]
public class AdminTariff
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server Server { get; set; } = null!;

    [Required]
    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Column("flags")]
    [MaxLength(100)]
    public string? Flags { get; set; }

    [Column("group_name")]
    [MaxLength(100)]
    public string? GroupName { get; set; }

    [Column("immunity")]
    public int Immunity { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("order")]
    public int Order { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public ICollection<AdminTariffOption> Options { get; set; } = new List<AdminTariffOption>();
}

[Table("donation_packages")]
public class DonationPackage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Column("suggested_amounts")]
    [MaxLength(500)]
    public string? SuggestedAmounts { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}

[Table("donation_transactions")]
public class DonationTransaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("transaction_id")]
    [MaxLength(255)]
    public string TransactionId { get; set; } = string.Empty; 

    [Column("user_id")]
    public int? UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Column("steam_id")]
    [MaxLength(50)]
    public string? SteamId { get; set; }

    [Required]
    [Column("amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; 

    [Column("tariff_id")]
    public int? TariffId { get; set; }

    [ForeignKey("TariffId")]
    public AdminTariff? Tariff { get; set; }

    [Column("tariff_option_id")]
    public int? TariffOptionId { get; set; }

    [ForeignKey("TariffOptionId")]
    public AdminTariffOption? TariffOption { get; set; }

    [Column("vip_tariff_id")]
    public int? VipTariffId { get; set; }

    [ForeignKey("VipTariffId")]
    public VipTariff? VipTariff { get; set; }

    [Column("vip_tariff_option_id")]
    public int? VipTariffOptionId { get; set; }

    [ForeignKey("VipTariffOptionId")]
    public VipTariffOption? VipTariffOption { get; set; }

    [Column("privilege_id")]
    public int? PrivilegeId { get; set; }

    [ForeignKey("PrivilegeId")]
    public UserAdminPrivilege? Privilege { get; set; }

    [Column("server_id")]
    public int? ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server? Server { get; set; }

    [Column("admin_password")]
    [MaxLength(100)]
    public string? AdminPassword { get; set; } 

    [Required]
    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "pending"; 

    [Column("payment_url")]
    [MaxLength(1000)]
    public string? PaymentUrl { get; set; } 

    [Column("pending_expires_at")]
    public DateTime? PendingExpiresAt { get; set; } 

    [Column("payment_method")]
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("label")]
    [MaxLength(255)]
    public string? Label { get; set; }

    [Column("expires_at")]
    public DateTime? ExpiresAt { get; set; } 

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("cancelled_at")]
    public DateTime? CancelledAt { get; set; }
}

[Table("vip_settings")]
public class VipSettings
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server Server { get; set; } = null!;

    [Required]
    [Column("host")]
    [MaxLength(255)]
    public string Host { get; set; } = string.Empty;

    [Required]
    [Column("port")]
    public int Port { get; set; } = 3306;

    [Required]
    [Column("database")]
    [MaxLength(100)]
    public string Database { get; set; } = string.Empty;

    [Required]
    [Column("username")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    [MaxLength(500)]
    public string Password { get; set; } = string.Empty;

    [Column("is_configured")]
    public bool IsConfigured { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}

[Table("vip_tariffs")]
public class VipTariff
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("server_id")]
    public int ServerId { get; set; }

    [ForeignKey("ServerId")]
    public Server Server { get; set; } = null!;

    [Required]
    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Column("group_name")]
    [MaxLength(64)]
    public string GroupName { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("order")]
    public int Order { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();

    public ICollection<VipTariffOption> Options { get; set; } = new List<VipTariffOption>();
}

[Table("vip_tariff_options")]
public class VipTariffOption
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("tariff_id")]
    public int TariffId { get; set; }

    [ForeignKey("TariffId")]
    public VipTariff Tariff { get; set; } = null!;

    [Required]
    [Column("duration_days")]
    public int DurationDays { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("order")]
    public int Order { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.GetServerLocalTime();
}
