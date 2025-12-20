using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Claims;
using System.Text.Json;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DonationController> _logger;
    private readonly IConfiguration _configuration;

    public DonationController(ApplicationDbContext context, ILogger<DonationController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }

    private string GenerateYooMoneyPaymentUrl(string walletNumber, decimal amount, string label, string targets, string successUrl)
    {
        return $"https://yoomoney.ru/quickpay/confirm?" +
               $"receiver={walletNumber}" +
               $"&quickpay-form=button" +
               $"&paymentType=PC" +
               $"&sum={amount}" +
               $"&label={label}" +
               $"&successURL={Uri.EscapeDataString(successUrl)}" +
               $"&targets={Uri.EscapeDataString(targets)}";
    }

    [HttpGet("settings")]
    public async Task<ActionResult> GetDonationSettings()
    {
        var yooMoneySettings = await _context.YooMoneySettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        var donationPackage = await _context.DonationPackages
            .Where(p => p.IsActive)
            .FirstOrDefaultAsync();

        if (yooMoneySettings == null)
        {
            return Ok(new { isConfigured = false });
        }

        List<int>? suggestedAmounts = null;
        if (donationPackage?.SuggestedAmounts != null)
        {
            try
            {
                suggestedAmounts = JsonSerializer.Deserialize<List<int>>(donationPackage.SuggestedAmounts);
            }
            catch { }
        }

        return Ok(new
        {
            isConfigured = true,
            walletNumber = yooMoneySettings.WalletNumber,
            donationPackage = donationPackage != null ? new
            {
                donationPackage.Id,
                donationPackage.Title,
                donationPackage.Description,
                suggestedAmounts
            } : null
        });
    }

    [HttpGet("tariffs")]
    public async Task<ActionResult<List<object>>> GetTariffsByServers()
    {
        var tariffs = await _context.AdminTariffs
            .Include(t => t.Server)
            .Include(t => t.Options.Where(o => o.IsActive))
            .Where(t => t.IsActive)
            .OrderBy(t => t.ServerId)
            .ThenBy(t => t.Order)
            .ToListAsync();

        var groupedByServer = tariffs
            .GroupBy(t => t.ServerId)
            .Select(g => new
            {
                serverId = g.Key,
                serverName = g.First().Server.Name,
                serverIp = $"{g.First().Server.IpAddress}:{g.First().Server.Port}",
                tariffs = g.Select(t => new
                {
                    Id = t.Id,
                    ServerId = t.ServerId,
                    ServerName = t.Server.Name,
                    Name = t.Name,
                    Description = t.Description,
                    Flags = t.Flags,
                    GroupName = t.GroupName,
                    Immunity = t.Immunity,
                    IsActive = t.IsActive,
                    Order = t.Order,
                    CreatedAt = t.CreatedAt,
                    Options = t.Options
                        .OrderBy(o => o.Order)
                        .Select(o => new
                        {
                            Id = o.Id,
                            DurationDays = o.DurationDays,
                            Price = o.Price,
                            Order = o.Order,
                            IsActive = o.IsActive
                        })
                        .ToList()
                }).ToList()
            })
            .ToList();

        return Ok(groupedByServer);
    }

    [Authorize]
    [HttpGet("my-privileges")]
    public async Task<ActionResult<List<UserAdminPrivilegeDto>>> GetMyPrivileges()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null || string.IsNullOrEmpty(user.SteamId))
        {
            return Ok(new List<UserAdminPrivilegeDto>());
        }

        var privileges = await _context.UserAdminPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .Where(p => p.UserId == userId.Value && p.SteamId == user.SteamId)
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.ExpiresAt)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = privileges.Select(p => new UserAdminPrivilegeDto
        {
            Id = p.Id,
            ServerId = p.ServerId,
            ServerName = p.Server?.Name ?? "Неизвестный сервер",
            TariffName = p.Tariff?.Name ?? "Неизвестный тариф",
            Flags = p.Flags,
            GroupName = p.GroupName,
            Immunity = p.Immunity,
            StartsAt = p.StartsAt,
            ExpiresAt = p.ExpiresAt,
            IsActive = p.IsActive && p.ExpiresAt > now,
            IsExpired = p.ExpiresAt <= now,
            DaysRemaining = p.ExpiresAt > now ? (int)(p.ExpiresAt - now).TotalDays : 0,
            AdminPassword = p.AdminPassword
        }).ToList();

        return Ok(result);
    }

    [Authorize]
    [HttpPost("create-donation")]
    public async Task<ActionResult> CreateDonation([FromBody] CreateDonationDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var yooMoneySettings = await _context.YooMoneySettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (yooMoneySettings == null)
        {
            return BadRequest(new { message = "Система доната не настроена" });
        }

        var transactionId = Guid.NewGuid().ToString();
        var label = $"donation_{userId}_{transactionId}";

        var frontendUrl = _configuration["FrontendUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var successUrl = $"{frontendUrl}/donation-success?type=donation&transactionId={transactionId}";
        var paymentUrl = GenerateYooMoneyPaymentUrl(
            yooMoneySettings.WalletNumber,
            dto.Amount,
            label,
            "Донат для проекта SibGamer",
            successUrl
        );

        var pendingExpiresAt = DateTimeHelper.GetServerLocalTime().AddMinutes(20);

        var transaction = new DonationTransaction
        {
            TransactionId = transactionId,
            UserId = userId.Value,
            Amount = dto.Amount,
            Type = "donation",
            Status = "pending",
            Label = label,
            PaymentUrl = paymentUrl,
            PendingExpiresAt = pendingExpiresAt
        };

        _context.DonationTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            transactionId,
            paymentUrl,
            amount = dto.Amount,
            pendingExpiresAt
        });
    }

    [Authorize]
    [HttpPost("create-admin-purchase")]
    public async Task<ActionResult> CreateAdminPurchase([FromBody] CreateAdminPurchaseDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null || string.IsNullOrEmpty(user.SteamId))
        {
            return BadRequest(new { message = "Необходимо добавить Steam ID в профиле" });
        }

        var yooMoneySettings = await _context.YooMoneySettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (yooMoneySettings == null)
        {
            return BadRequest(new { message = "Система доната не настроена" });
        }

        var tariffOption = await _context.AdminTariffOptions
            .Include(o => o.Tariff)
            .ThenInclude(t => t.Server)
            .FirstOrDefaultAsync(o => o.Id == dto.TariffOptionId && o.IsActive && o.Tariff.IsActive);

        if (tariffOption == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        if (tariffOption.Tariff.ServerId != dto.ServerId)
        {
            return BadRequest(new { message = "Тариф не соответствует выбранному серверу" });
        }

        var now = DateTimeHelper.GetServerLocalTime();
        var hasActiveAdminPrivilege = await _context.UserAdminPrivileges
            .AnyAsync(p => p.UserId == userId.Value && p.ServerId == dto.ServerId && p.IsActive && p.ExpiresAt > now);

        if (hasActiveAdminPrivilege)
        {
            return BadRequest(new { message = "У вас уже есть активная админ-привилегия на этом сервере. Вы сможете купить новую только после окончания текущей услуги." });
        }

        var transactionId = Guid.NewGuid().ToString();
        var label = $"admin_{userId}_{dto.ServerId}_{dto.TariffOptionId}_{transactionId}";

        var expiresAt = tariffOption.DurationDays == 0 
            ? DateTime.MaxValue 
            : DateTimeHelper.GetServerLocalTime().AddDays(tariffOption.DurationDays);

        var frontendUrl = _configuration["FrontendUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var successUrl = $"{frontendUrl}/donation-success?type=admin&transactionId={transactionId}";
        var paymentUrl = GenerateYooMoneyPaymentUrl(
            yooMoneySettings.WalletNumber,
            tariffOption.Price,
            label,
            $"Покупка {tariffOption.Tariff.Name} на сервере {tariffOption.Tariff.Server.Name}",
            successUrl
        );

        var pendingExpiresAt = DateTimeHelper.GetServerLocalTime().AddMinutes(20);

        var transaction = new DonationTransaction
        {
            TransactionId = transactionId,
            UserId = userId.Value,
            SteamId = user.SteamId,
            Amount = tariffOption.Price,
            Type = "admin_purchase",
            TariffId = tariffOption.TariffId,
            TariffOptionId = dto.TariffOptionId,
            ServerId = dto.ServerId,
            AdminPassword = dto.AdminPassword,
            Status = "pending",
            Label = label,
            ExpiresAt = expiresAt,
            PaymentUrl = paymentUrl,
            PendingExpiresAt = pendingExpiresAt
        };

        _context.DonationTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            transactionId,
            paymentUrl,
            amount = tariffOption.Price,
            tariffName = tariffOption.Tariff.Name,
            serverName = tariffOption.Tariff.Server.Name,
            pendingExpiresAt
        });
    }

    [Authorize]
    [HttpGet("transaction/{transactionId}")]
    public async Task<ActionResult> GetTransactionStatus(string transactionId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var transaction = await _context.DonationTransactions
            .Include(t => t.Tariff)
            .Include(t => t.Server)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId.Value);

        if (transaction == null)
        {
            return NotFound(new { message = "Транзакция не найдена" });
        }

        return Ok(new
        {
            transaction.TransactionId,
            transaction.Status,
            transaction.Amount,
            transaction.Type,
            tariffName = transaction.Tariff?.Name,
            serverName = transaction.Server?.Name,
            transaction.CreatedAt,
            transaction.CompletedAt,
            transaction.PendingExpiresAt,
            transaction.AdminPassword
        });
    }

    [Authorize]
    [HttpGet("history")]
    public async Task<ActionResult> GetUserTransactionHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var query = _context.DonationTransactions
            .Include(t => t.Tariff)
            .Include(t => t.Server)
            .Where(t => t.UserId == userId.Value)
            .OrderByDescending(t => t.CreatedAt);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.TransactionId,
                t.Status,
                t.Amount,
                t.Type,
                tariffName = t.Tariff != null ? t.Tariff.Name : null,
                serverName = t.Server != null ? t.Server.Name : null,
                t.CreatedAt,
                t.CompletedAt,
                t.PendingExpiresAt
            })
            .ToListAsync();

        return Ok(new
        {
            items,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [Authorize]
    [HttpPost("cancel-transaction/{transactionId}")]
    public async Task<ActionResult> CancelTransaction(string transactionId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var transaction = await _context.DonationTransactions
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId.Value);

        if (transaction == null)
        {
            return NotFound(new { message = "Транзакция не найдена" });
        }

        if (transaction.Status != "pending")
        {
            return BadRequest(new { message = "Только ожидающие платежи могут быть отменены" });
        }

        transaction.Status = "cancelled";
        transaction.CancelledAt = DateTimeHelper.GetServerLocalTime();
        await _context.SaveChangesAsync();

        return Ok(new { message = "Транзакция отменена" });
    }

    [HttpGet("top-donators")]
    public async Task<ActionResult> GetTopDonators([FromQuery] int limit = 3)
    {
        var topDonators = await _context.DonationTransactions
            .Where(t => t.Status == "completed" && t.UserId != null)
            .GroupBy(t => new { t.UserId, t.User!.Username, t.User.AvatarUrl })
            .Select(g => new
            {
                userId = g.Key.UserId,
                username = g.Key.Username,
                avatarUrl = g.Key.AvatarUrl,
                totalAmount = g.Sum(t => t.Amount),
                donationCount = g.Count()
            })
            .OrderByDescending(x => x.totalAmount)
            .Take(limit)
            .ToListAsync();

        return Ok(topDonators);
    }

    [Authorize]
    [HttpGet("extend-options/{privilegeId}")]
    public async Task<ActionResult<ExtendOptionsDto>> GetExtendOptions(int privilegeId)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var privilege = await _context.UserAdminPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .FirstOrDefaultAsync(p => p.Id == privilegeId && p.UserId == userId.Value && p.IsActive);

        if (privilege == null)
        {
            return NotFound(new { message = "Привилегия не найдена или не активна" });
        }

        var options = await _context.AdminTariffOptions
            .Where(o => o.TariffId == privilege.TariffId && o.IsActive)
            .OrderBy(o => o.Order)
            .Select(o => new AdminTariffOptionDto
            {
                Id = o.Id,
                TariffId = o.TariffId,
                DurationDays = o.DurationDays,
                Price = o.Price,
                Order = o.Order,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            })
            .ToListAsync();

        return Ok(new ExtendOptionsDto
        {
            PrivilegeId = privilegeId,
            AvailableOptions = options
        });
    }

    [Authorize]
    [HttpPost("extend-admin-privilege")]
    public async Task<ActionResult> ExtendAdminPrivilege(ExtendAdminPrivilegeDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var privilege = await _context.UserAdminPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .FirstOrDefaultAsync(p => p.Id == dto.PrivilegeId && p.UserId == userId.Value && p.IsActive);

        if (privilege == null)
        {
            return NotFound(new { message = "Привилегия не найдена или не активна" });
        }

        var tariffOption = await _context.AdminTariffOptions
            .FirstOrDefaultAsync(o => o.Id == dto.TariffOptionId && o.TariffId == privilege.TariffId && o.IsActive);

        if (tariffOption == null)
        {
            return BadRequest(new { message = "Вариант продления не найден" });
        }

        var existingPendingTransaction = await _context.DonationTransactions
            .FirstOrDefaultAsync(t => t.UserId == userId.Value && 
                                    t.TariffId == privilege.TariffId && 
                                    t.ServerId == privilege.ServerId && 
                                    t.Status == "pending" &&
                                    t.Type == "extend");

        if (existingPendingTransaction != null)
        {
            return BadRequest(new { message = "У вас уже есть ожидающий платеж для продления этой привилегии" });
        }

        var conflictingPrivilege = await _context.UserAdminPrivileges
            .FirstOrDefaultAsync(p => p.UserId == userId.Value && 
                                    p.ServerId == privilege.ServerId && 
                                    p.IsActive && 
                                    p.Id != privilege.Id &&
                                    ((p.StartsAt <= privilege.ExpiresAt && p.ExpiresAt >= privilege.StartsAt) ||
                                     (p.ExpiresAt >= privilege.StartsAt && p.StartsAt <= privilege.ExpiresAt)));

        if (conflictingPrivilege != null)
        {
            return BadRequest(new { message = "У вас уже есть активная привилегия на этом сервере, которая конфликтует с продлением" });
        }

        var transactionId = Guid.NewGuid().ToString();
        var transaction = new DonationTransaction
        {
            TransactionId = transactionId,
            UserId = userId.Value,
            Amount = tariffOption.Price,
            Type = "extend",
            TariffId = privilege.TariffId,
            ServerId = privilege.ServerId,
            Status = "pending",
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            PendingExpiresAt = DateTimeHelper.GetServerLocalTime().AddMinutes(30),
            PrivilegeId = privilege.Id,
            TariffOptionId = tariffOption.Id,
            AdminPassword = dto.AdminPassword 
        };

        _context.DonationTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        var yooMoneySettings = await _context.YooMoneySettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (yooMoneySettings == null)
        {
            return BadRequest(new { message = "Система оплаты не настроена" });
        }

        var frontendUrl = _configuration["FrontendUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var successUrl = $"{frontendUrl}/donation-success?type=extend&transactionId={transactionId}";
        var paymentUrl = GenerateYooMoneyPaymentUrl(
            yooMoneySettings.WalletNumber,
            tariffOption.Price,
            transactionId,
            $"Продление привилегии '{privilege.Tariff?.Name ?? "Неизвестный тариф"}' на сервере '{privilege.Server?.Name ?? "Неизвестный сервер"}' на {tariffOption.DurationDays} дней",
            successUrl
        );

        return Ok(new
        {
            transactionId,
            paymentUrl,
            amount = tariffOption.Price,
            description = $"Продление привилегии '{privilege.Tariff?.Name ?? "Неизвестный тариф"}' на сервере '{privilege.Server?.Name ?? "Неизвестный сервер"}' на {tariffOption.DurationDays} дней"
        });
    }

    [HttpGet("vip-tariffs")]
    public async Task<ActionResult<List<object>>> GetVipTariffsByServers()
    {
        var tariffs = await _context.VipTariffs
            .Include(t => t.Server)
            .Include(t => t.Options.Where(o => o.IsActive))
            .Where(t => t.IsActive)
            .OrderBy(t => t.ServerId)
            .ThenBy(t => t.Order)
            .ToListAsync();

        var groupedByServer = tariffs
            .GroupBy(t => t.ServerId)
            .Select(g => new
            {
                serverId = g.Key,
                serverName = g.First().Server.Name,
                serverIp = $"{g.First().Server.IpAddress}:{g.First().Server.Port}",
                tariffs = g.Select(t => new
                {
                    Id = t.Id,
                    ServerId = t.ServerId,
                    ServerName = t.Server.Name,
                    Name = t.Name,
                    Description = t.Description,
                    GroupName = t.GroupName,
                    IsActive = t.IsActive,
                    Order = t.Order,
                    CreatedAt = t.CreatedAt,
                    Options = t.Options
                        .OrderBy(o => o.Order)
                        .Select(o => new
                        {
                            Id = o.Id,
                            DurationDays = o.DurationDays,
                            Price = o.Price,
                            Order = o.Order,
                            IsActive = o.IsActive
                        })
                        .ToList()
                }).ToList()
            })
            .ToList();

        return Ok(groupedByServer);
    }

    [HttpGet("servers-with-vip")]
    public async Task<ActionResult<List<object>>> GetServersWithVipPublic()
    {
        var serversWithVip = await _context.VipSettings
            .Where(s => s.IsConfigured)
            .Include(s => s.Server)
            .Select(s => new
            {
                serverId = s.ServerId,
                serverName = s.Server.Name,
                ipAddress = $"{s.Server.IpAddress}:{s.Server.Port}"
            })
            .ToListAsync();

        return Ok(serversWithVip);
    }

    [Authorize]
    [HttpGet("my-vip-privileges")]
    public async Task<ActionResult<List<UserVipPrivilegeDto>>> GetMyVipPrivileges()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null || string.IsNullOrEmpty(user.SteamId))
        {
            return Ok(new List<UserVipPrivilegeDto>());
        }

        var privileges = await _context.UserVipPrivileges
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .Where(p => p.UserId == userId.Value && p.SteamId == user.SteamId)
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.ExpiresAt)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = privileges.Select(p => new UserVipPrivilegeDto
        {
            Id = p.Id,
            ServerId = p.ServerId,
            ServerName = p.Server?.Name ?? "Неизвестный сервер",
            TariffName = p.Tariff?.Name ?? "Неизвестный тариф",
            GroupName = p.GroupName,
            StartsAt = p.StartsAt,
            ExpiresAt = p.ExpiresAt,
            IsActive = p.IsActive && p.ExpiresAt > now,
            IsExpired = p.ExpiresAt <= now,
            DaysRemaining = p.ExpiresAt > now ? (int)(p.ExpiresAt - now).TotalDays : 0
        }).ToList();

        return Ok(result);
    }

    [Authorize]
    [HttpPost("create-vip-purchase")]
    public async Task<ActionResult> CreateVipPurchase([FromBody] CreateVipPurchaseDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null || string.IsNullOrEmpty(user.SteamId))
        {
            return BadRequest(new { message = "Необходимо добавить Steam ID в профиле" });
        }

        var yooMoneySettings = await _context.YooMoneySettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (yooMoneySettings == null)
        {
            return BadRequest(new { message = "Система доната не настроена" });
        }

        var tariffOption = await _context.VipTariffOptions
            .Include(o => o.Tariff)
            .ThenInclude(t => t.Server)
            .FirstOrDefaultAsync(o => o.Id == dto.TariffOptionId && o.IsActive && o.Tariff.IsActive);

        if (tariffOption == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        if (tariffOption.Tariff.ServerId != dto.ServerId)
        {
            return BadRequest(new { message = "Тариф не соответствует выбранному серверу" });
        }

        var transactionId = Guid.NewGuid().ToString();
        var label = $"vip_{userId}_{dto.ServerId}_{dto.TariffOptionId}_{transactionId}";

        var expiresAt = tariffOption.DurationDays == 0 
            ? DateTime.MaxValue 
            : DateTimeHelper.GetServerLocalTime().AddDays(tariffOption.DurationDays);

        var frontendUrl = _configuration["FrontendUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var successUrl = $"{frontendUrl}/donation-success?type=vip&transactionId={transactionId}";
        var paymentUrl = GenerateYooMoneyPaymentUrl(
            yooMoneySettings.WalletNumber,
            tariffOption.Price,
            label,
            $"Покупка VIP {tariffOption.Tariff.Name} на сервере {tariffOption.Tariff.Server.Name}",
            successUrl
        );

        var pendingExpiresAt = DateTimeHelper.GetServerLocalTime().AddMinutes(20);
        var now = DateTimeHelper.GetServerLocalTime();
        var hasActiveVipPrivilege = await _context.UserVipPrivileges
            .AnyAsync(p => p.UserId == userId.Value && p.ServerId == dto.ServerId && p.IsActive && p.ExpiresAt > now);

        if (hasActiveVipPrivilege)
        {
            return BadRequest(new { message = "У вас уже есть активная VIP-привилегия на этом сервере. Вы сможете купить новую только после окончания текущей услуги." });
        }

        var transaction = new DonationTransaction
        {
            TransactionId = transactionId,
            UserId = userId.Value,
            SteamId = user.SteamId,
            Amount = tariffOption.Price,
            Type = "vip_purchase",
            VipTariffId = tariffOption.TariffId,
            VipTariffOptionId = dto.TariffOptionId,
            ServerId = dto.ServerId,
            Status = "pending",
            Label = label,
            ExpiresAt = expiresAt,
            PaymentUrl = paymentUrl,
            PendingExpiresAt = pendingExpiresAt
        };

        _context.DonationTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            transactionId,
            paymentUrl,
            amount = tariffOption.Price,
            tariffName = tariffOption.Tariff.Name,
            serverName = tariffOption.Tariff.Server.Name,
            pendingExpiresAt
        });
    }

    [Authorize]
    [HttpPost("vip-application")]
    public async Task<ActionResult> CreateVipApplication([FromBody] CreateVipApplicationDto dto)
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null || string.IsNullOrEmpty(user.SteamId))
        {
            return BadRequest(new { message = "Необходимо добавить Steam ID в профиле" });
        }

        var server = await _context.Servers.FindAsync(dto.ServerId);
        if (server == null)
            return NotFound(new { message = "Сервер не найден" });

        var application = new Models.VipApplication
        {
            UserId = userId.Value,
            Username = user.Username,
            SteamId = user.SteamId,
            ServerId = dto.ServerId,
            HoursPerWeek = dto.HoursPerWeek,
            Reason = dto.Reason,
            Status = "pending",
            CreatedAt = DateTimeHelper.GetServerLocalTime(),
            UpdatedAt = DateTimeHelper.GetServerLocalTime()
        };

        _context.VipApplications.Add(application);
        await _context.SaveChangesAsync();

        var notificationService = HttpContext.RequestServices.GetRequiredService<INotificationService>();
        var title = "Новая заявка на участника";
        var message = $"Пользователь {user.Username} подал заявку на участника на сервере {server.Name}. Причина: {dto.Reason}";
        await notificationService.CreateAdminNotificationAsync(title, message, "vip_application", application.Id);

        return Ok(new { message = "Заявка отправлена" , applicationId = application.Id});
    }

    [Authorize]
    [HttpGet("my-vip-applications")]
    public async Task<ActionResult<List<VipApplicationDto>>> GetMyVipApplications()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var apps = await _context.VipApplications
            .Include(a => a.Server)
            .Where(a => a.UserId == userId.Value)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var result = apps.Select(a => new VipApplicationDto
        {
            Id = a.Id,
            UserId = a.UserId,
            Username = a.Username,
            SteamId = a.SteamId,
            ServerId = a.ServerId,
            ServerName = a.Server?.Name ?? "Неизвестный сервер",
            HoursPerWeek = a.HoursPerWeek,
            Reason = a.Reason,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            ProcessedAt = a.ProcessedAt
        }).ToList();

        return Ok(result);
    }
}
