using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace backend.DTOs;

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? SteamId { get; set; }
    public string? SteamProfileUrl { get; set; }
    public string? LastIp { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? BlockedAt { get; set; }
    public string? BlockReason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserDetailsDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? SteamId { get; set; }
    public string? SteamProfileUrl { get; set; }
    public string? LastIp { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? BlockedAt { get; set; }
    public string? BlockReason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ServerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string MapName { get; set; } = string.Empty;
    public int CurrentPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public bool IsOnline { get; set; }
    public bool RconPasswordSet { get; set; }
}

public class CreateServerDto
{
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? Name { get; set; }
    public string? MapName { get; set; }
    public int? MaxPlayers { get; set; }
    [MaxLength(100)]
    public string? RconPassword { get; set; }
}

public class UpdateUsernameDto
{
    public string NewUsername { get; set; } = string.Empty;
}

public class UpdatePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class UpdateAvatarDto
{
    public string AvatarUrl { get; set; } = string.Empty;
}

public class UpdateSteamDto
{
    [Required]
    public string SteamInput { get; set; } = string.Empty;
}

public class NewsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public UserDto Author { get; set; } = null!;
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public List<NewsMediaDto> Media { get; set; } = new();
    public bool IsLikedByCurrentUser { get; set; }
}

public class NewsListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsPublished { get; set; }
}

public class CreateNewsDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public bool IsPublished { get; set; } = true;
    public List<string>? MediaUrls { get; set; }
}

public class UpdateNewsDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Summary { get; set; }
    public string? Slug { get; set; }
    public string? CoverImage { get; set; }
    public bool? IsPublished { get; set; }
    public List<string>? MediaUrls { get; set; }
}

public class NewsMediaDto
{
    public int Id { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class NewsCommentDto
{
    public int Id { get; set; }
    public int NewsId { get; set; }
    public UserDto User { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
    public ParentCommentInfo? ParentComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<NewsCommentDto> Replies { get; set; } = new();
}

public class ParentCommentInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ContentPreview { get; set; } = string.Empty;
}

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}

public class SmtpSettingsDto
{
    public int Id { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public bool IsConfigured { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateSmtpSettingsDto
{
    [Required]
    public string Host { get; set; } = string.Empty;
    
    [Required]
    public int Port { get; set; }
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public bool EnableSsl { get; set; } = true;
    
    [Required]
    [EmailAddress]
    public string FromEmail { get; set; } = string.Empty;
    
    [Required]
    public string FromName { get; set; } = string.Empty;
}

public class BulkEmailDto
{
    [Required]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string Body { get; set; } = string.Empty;
}

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}

public class TestEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string TestEmailAddress { get; set; } = string.Empty;
}

public class TestEmailResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class BulkEmailResponseDto
{
    public int TotalRecipients { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string> Errors { get; set; } = new();
}


public class YooMoneySettingsDto
{
    public int Id { get; set; }
    public string WalletNumber { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool IsConfigured { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateYooMoneySettingsDto
{
    [Required]
    public string WalletNumber { get; set; } = string.Empty;
    
    [Required]
    public string SecretKey { get; set; } = string.Empty;
}

public class SourceBansSettingsDto
{
    public int Id { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsConfigured { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateSourceBansSettingsDto
{
    public int? ServerId { get; set; }
    
    [Required]
    public string Host { get; set; } = string.Empty;
    
    [Required]
    public int Port { get; set; } = 3306;
    
    [Required]
    public string Database { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AdminTariffDto
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public string? Flags { get; set; }
    public string? GroupName { get; set; }
    public int Immunity { get; set; }
    public bool IsActive { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AdminTariffOptionDto>? Options { get; set; }
}

public class CreateAdminTariffDto
{
    [Required]
    public int ServerId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 3650)]
    public int? DurationDays { get; set; }
    
    [Range(0.01, 1000000)]
    public decimal? Price { get; set; }
    
    [MaxLength(100)]
    public string? Flags { get; set; }
    
    [MaxLength(100)]
    public string? GroupName { get; set; }
    
    [Range(0, 100)]
    public int Immunity { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
    
    public int Order { get; set; } = 0;
}

public class UpdateAdminTariffDto
{
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Range(1, 3650)]
    public int? DurationDays { get; set; }
    
    [Range(0.01, 1000000)]
    public decimal? Price { get; set; }
    
    [MaxLength(100)]
    public string? Flags { get; set; }
    
    [MaxLength(100)]
    public string? GroupName { get; set; }
    
    [Range(0, 100)]
    public int? Immunity { get; set; }
    
    public bool? IsActive { get; set; }
    
    public int? Order { get; set; }
}

public class DonationPackageDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<int>? SuggestedAmounts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateDonationPackageDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    public List<int>? SuggestedAmounts { get; set; }
    
    public bool IsActive { get; set; } = true;
}

public class CreateDonationDto
{
    [Required]
    [Range(1, 1000000)]
    public decimal Amount { get; set; }
    
    public string? Message { get; set; }
}

public class CreateAdminPurchaseDto
{
    [Required]
    public int TariffOptionId { get; set; }
    
    [Required]
    public int ServerId { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    public string AdminPassword { get; set; } = string.Empty;
}

public class DonationTransactionDto
{
    public int Id { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public string? SteamId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? TariffId { get; set; }
    public string? TariffName { get; set; }
    public int? ServerId { get; set; }
    public string? ServerName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class UserAdminPrivilegeDto
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public string? Flags { get; set; }
    public string? GroupName { get; set; }
    public int Immunity { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysRemaining { get; set; }
    public string? AdminPassword { get; set; }
}

public class YooMoneyWebhookDto
{
    public string? notification_type { get; set; }
    public string? operation_id { get; set; }
    public string? amount { get; set; }
    public string? withdraw_amount { get; set; }
    public string? currency { get; set; }
    public string? datetime { get; set; }
    public string? sender { get; set; }
    public string? codepro { get; set; }
    public string? label { get; set; }
    public string? sha1_hash { get; set; }
    public string? unaccepted { get; set; }
    public string? test_notification { get; set; }
    public string? lastname { get; set; }
    public string? firstname { get; set; }
    public string? fathersname { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public string? city { get; set; }
    public string? street { get; set; }
    public string? building { get; set; }
    public string? suite { get; set; }
    public string? flat { get; set; }
    public string? zip { get; set; }
}

public class AdminTariffOptionDto
{
    public int Id { get; set; }
    public int TariffId { get; set; }
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAdminTariffOptionDto
{
    [Required]
    [Range(0, 3650)] 
    public int DurationDays { get; set; }
    
    [Required]
    [Range(0.01, 1000000)]
    public decimal Price { get; set; }
    
    public int Order { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
}

public class UpdateAdminPasswordDto
{
    [Required]
    public int PrivilegeId { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;
}

public class UpdateSteamIdDto
{
    [Required]
    public string SteamInput { get; set; } = string.Empty;
}

public class BlockUserDto
{
    public string? Reason { get; set; }
}

public class UpdateAdminTariffOptionDto
{
    [Range(0, 3650)] 
    public int? DurationDays { get; set; }
    
    [Range(0.01, 1000000)]
    public decimal? Price { get; set; }
    
    public int? Order { get; set; }
    
    public bool? IsActive { get; set; }
}

public class SendNotificationDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
}

public class SendNotificationResponseDto
{
    public int TotalRecipients { get; set; }
    public int SuccessCount { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ExtendAdminPrivilegeDto
{
    [Required]
    public int PrivilegeId { get; set; }
    
    [Required]
    public int TariffOptionId { get; set; }
    
    [MinLength(4)]
    [MaxLength(100)]
    public string? AdminPassword { get; set; } 
}

public class ExtendOptionsDto
{
    public int PrivilegeId { get; set; }
    public List<AdminTariffOptionDto> AvailableOptions { get; set; } = new();
}


public class VipSettingsDto
{
    public int Id { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsConfigured { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateVipSettingsDto
{
    public int? ServerId { get; set; }
    
    [Required]
    public string Host { get; set; } = string.Empty;
    
    [Required]
    public int Port { get; set; } = 3306;
    
    [Required]
    public string Database { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class VipTariffDto
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<VipTariffOptionDto>? Options { get; set; }
}

public class CreateVipTariffDto
{
    [Required]
    public int ServerId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(64)]
    public string GroupName { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public int Order { get; set; } = 0;
}

public class UpdateVipTariffDto
{
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [MaxLength(64)]
    public string? GroupName { get; set; }
    
    public bool? IsActive { get; set; }
    
    public int? Order { get; set; }
}

public class VipTariffOptionDto
{
    public int Id { get; set; }
    public int TariffId { get; set; }
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateVipTariffOptionDto
{
    [Required]
    [Range(0, 3650)] 
    public int DurationDays { get; set; }
    
    [Required]
    [Range(0.01, 1000000)]
    public decimal Price { get; set; }
    
    public int Order { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
}

public class UpdateVipTariffOptionDto
{
    [Range(0, 3650)] 
    public int? DurationDays { get; set; }
    
    [Range(0.01, 1000000)]
    public decimal? Price { get; set; }
    
    public int? Order { get; set; }
    
    public bool? IsActive { get; set; }
}

public class CreateVipPurchaseDto
{
    [Required]
    public int TariffOptionId { get; set; }
    
    [Required]
    public int ServerId { get; set; }
}

public class CreateVipApplicationDto
{
    [Required]
    public int ServerId { get; set; }

    [Range(0, 168)]
    public int? HoursPerWeek { get; set; }

    [Required]
    [StringLength(2000)]
    public string Reason { get; set; } = string.Empty;
}

public class VipApplicationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string SteamId { get; set; } = string.Empty;
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public int? HoursPerWeek { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? AdminId { get; set; }
    public string? AdminComment { get; set; }
    public string? VipGroup { get; set; }
    public int? DurationDays { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

public class AdminApproveVipApplicationDto
{
    [Required]
    public string VipGroup { get; set; } = string.Empty;

    [Range(0, 3650)]
    public int DurationDays { get; set; }
    public int? TariffId { get; set; }
    public int? TariffOptionId { get; set; }
}

public class AdminRejectVipApplicationDto
{
    [Required]
    [StringLength(2000)]
    public string Reason { get; set; } = string.Empty;
}

public class UserVipPrivilegeDto
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string TariffName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public int DaysRemaining { get; set; }
}

public class CustomPageDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public UserDto Author { get; set; } = null!;
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public List<CustomPageMediaDto> Media { get; set; } = new();
}

public class CustomPageListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsPublished { get; set; }
}

public class CustomPageMediaDto
{
    public int Id { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public int Order { get; set; }
}
