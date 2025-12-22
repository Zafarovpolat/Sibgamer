using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Claims;
using MySqlConnector;

namespace backend.Controllers.Admin;

[ApiController]
[Route("api/admin/donation")]
[Authorize(Roles = "Admin")]
public class AdminDonationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISourceBansService _sourceBansService;
    private readonly ILogger<AdminDonationController> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _frontendUrl;

    public AdminDonationController(
        ApplicationDbContext context,
        ISourceBansService sourceBansService,
        ILogger<AdminDonationController> logger,
        IConfiguration configuration)
    {
        _context = context;
        _sourceBansService = sourceBansService;
        _logger = logger;
        _configuration = configuration;
        _frontendUrl = configuration["FrontendUrl"] ?? "https://sibgamer.com";
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? int.Parse(userIdClaim) : null;
    }

    #region YooMoney Settings

    [HttpGet("yoomoney-settings")]
    public async Task<ActionResult<YooMoneySettingsDto>> GetYooMoneySettings()
    {
        var settings = await _context.YooMoneySettings
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            return Ok(new YooMoneySettingsDto { IsConfigured = false });
        }

        return Ok(new YooMoneySettingsDto
        {
            Id = settings.Id,
            WalletNumber = settings.WalletNumber,
            SecretKey = settings.SecretKey,
            IsConfigured = settings.IsConfigured,
            UpdatedAt = settings.UpdatedAt
        });
    }

    [HttpPost("yoomoney-settings")]
    public async Task<ActionResult<YooMoneySettingsDto>> UpdateYooMoneySettings([FromBody] UpdateYooMoneySettingsDto dto)
    {
        var settings = await _context.YooMoneySettings
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            settings = new YooMoneySettings
            {
                WalletNumber = dto.WalletNumber,
                SecretKey = dto.SecretKey,
                IsConfigured = true,
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.YooMoneySettings.Add(settings);
        }
        else
        {
            settings.WalletNumber = dto.WalletNumber;
            settings.SecretKey = dto.SecretKey;
            settings.IsConfigured = true;
            settings.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();

        return Ok(new YooMoneySettingsDto
        {
            Id = settings.Id,
            WalletNumber = settings.WalletNumber,
            SecretKey = settings.SecretKey,
            IsConfigured = settings.IsConfigured,
            UpdatedAt = settings.UpdatedAt
        });
    }

    #endregion

    #region SourceBans Settings (per server)

    [HttpGet("sourcebans-settings")]
    public async Task<ActionResult> GetAllSourceBansSettings()
    {
        var settings = await _context.SourceBansSettings
            .Include(s => s.Server)
            .OrderBy(s => s.Server.Name)
            .Select(s => new
            {
                s.Id,
                s.ServerId,
                serverName = s.Server.Name,
                s.Host,
                s.Port,
                s.Database,
                s.Username,
                s.Password,
                s.IsConfigured,
                s.CreatedAt,
                s.UpdatedAt
            })
            .ToListAsync();

        return Ok(settings);
    }

    [HttpGet("sourcebans-settings/{serverId}")]
    public async Task<ActionResult> GetSourceBansSettingsByServer(int serverId)
    {
        var settings = await _context.SourceBansSettings
            .Include(s => s.Server)
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return Ok(new { isConfigured = false, serverId });
        }

        return Ok(new
        {
            settings.Id,
            settings.ServerId,
            serverName = settings.Server.Name,
            settings.Host,
            settings.Port,
            settings.Database,
            settings.Username,
            settings.Password,
            settings.IsConfigured,
            settings.CreatedAt,
            settings.UpdatedAt
        });
    }

    [HttpPost("sourcebans-settings")]
    public async Task<ActionResult> UpsertSourceBansSettings([FromBody] UpdateSourceBansSettingsDto dto)
    {
        if (!dto.ServerId.HasValue)
        {
            return BadRequest(new { message = "ServerId обязателен" });
        }

        var server = await _context.Servers.FindAsync(dto.ServerId.Value);
        if (server == null)
        {
            return NotFound(new { message = "Сервер не найден" });
        }

        var settings = await _context.SourceBansSettings
            .FirstOrDefaultAsync(s => s.ServerId == dto.ServerId.Value);

        if (settings == null)
        {
            settings = new SourceBansSettings
            {
                ServerId = dto.ServerId.Value,
                Host = dto.Host,
                Port = dto.Port,
                Database = dto.Database,
                Username = dto.Username,
                Password = dto.Password,
                IsConfigured = true,
                CreatedAt = DateTimeHelper.GetServerLocalTime(),
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.SourceBansSettings.Add(settings);
        }
        else
        {
            settings.Host = dto.Host;
            settings.Port = dto.Port;
            settings.Database = dto.Database;
            settings.Username = dto.Username;
            settings.Password = dto.Password;
            settings.IsConfigured = true;
            settings.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            settings.Id,
            settings.ServerId,
            serverName = server.Name,
            settings.Host,
            settings.Port,
            settings.Database,
            settings.Username,
            settings.Password,
            settings.IsConfigured,
            settings.CreatedAt,
            settings.UpdatedAt
        });
    }

    [HttpDelete("sourcebans-settings/{serverId}")]
    public async Task<ActionResult> DeleteSourceBansSettings(int serverId)
    {
        var settings = await _context.SourceBansSettings
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return NotFound(new { message = "Настройки не найдены" });
        }

        _context.SourceBansSettings.Remove(settings);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Настройки удалены" });
    }

    [HttpPost("sourcebans-settings/{serverId}/test")]
    public async Task<ActionResult> TestSourceBansConnection(int serverId)
    {
        var settings = await _context.SourceBansSettings
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return NotFound(new { success = false, message = "Настройки SourceBans для этого сервера не найдены" });
        }

        try
        {
            var connectionString = $"Server={settings.Host};Port={settings.Port};Database={settings.Database};Uid={settings.Username};Pwd={settings.Password};";
            using var connection = new MySqlConnector.MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SHOW TABLES LIKE 'sb_admins'";
            var result = await cmd.ExecuteScalarAsync();
            
            if (result == null)
            {
                return Ok(new { success = false, message = "Таблица sb_admins не найдена в базе данных" });
            }

            return Ok(new { success = true, message = "Подключение успешно! Таблица sb_admins найдена." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при тестировании подключения к SourceBans для сервера {ServerId}", serverId);
            return Ok(new { success = false, message = $"Ошибка подключения: {ex.Message}" });
        }
    }

    #endregion

    #region Tariffs

    [HttpGet("servers-with-sourcebans")]
    public async Task<ActionResult<List<object>>> GetServersWithSourceBans()
    {
        var serversWithSourceBans = await _context.SourceBansSettings
            .Where(s => s.IsConfigured)
            .Include(s => s.Server)
            .Select(s => new
            {
                id = s.ServerId,
                name = s.Server.Name,
                ipAddress = $"{s.Server.IpAddress}:{s.Server.Port}"
            })
            .ToListAsync();

        return Ok(serversWithSourceBans);
    }

    [HttpGet("tariffs")]
    public async Task<ActionResult<List<AdminTariffDto>>> GetTariffs()
    {
        var tariffs = await _context.AdminTariffs
            .Include(t => t.Server)
            .Include(t => t.Options)
            .OrderBy(t => t.ServerId)
            .ThenBy(t => t.Order)
            .ToListAsync();

        var result = tariffs.Select(t => new AdminTariffDto
        {
            Id = t.Id,
            ServerId = t.ServerId,
            ServerName = t.Server.Name,
            Name = t.Name,
            Description = t.Description,
            DurationDays = 0, 
            Price = 0, 
            Flags = t.Flags,
            GroupName = t.GroupName,
            Immunity = t.Immunity,
            IsActive = t.IsActive,
            Order = t.Order,
            CreatedAt = t.CreatedAt,
            Options = t.Options?.Select(o => new AdminTariffOptionDto
            {
                Id = o.Id,
                TariffId = o.TariffId,
                DurationDays = o.DurationDays,
                Price = o.Price,
                Order = o.Order,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    [HttpPost("tariffs")]
    public async Task<ActionResult<AdminTariffDto>> CreateTariff([FromBody] CreateAdminTariffDto dto)
    {
        var server = await _context.Servers.FindAsync(dto.ServerId);
        if (server == null)
        {
            return NotFound(new { message = "Сервер не найден" });
        }

        var sourceBansSettings = await _context.SourceBansSettings
            .FirstOrDefaultAsync(s => s.ServerId == dto.ServerId && s.IsConfigured);

        if (sourceBansSettings == null)
        {
            return BadRequest(new { message = "Для этого сервера не настроен SourceBans. Сначала настройте подключение к базе данных." });
        }

        if (!string.IsNullOrWhiteSpace(dto.Flags) && !string.IsNullOrWhiteSpace(dto.GroupName))
        {
            return BadRequest(new { message = "Укажите либо флаги (srv_flags), либо группу (srv_group). В SourceBans используется только один вариант." });
        }

        if (string.IsNullOrWhiteSpace(dto.Flags) && string.IsNullOrWhiteSpace(dto.GroupName))
        {
            return BadRequest(new { message = "Укажите либо флаги (srv_flags), либо группу (srv_group)." });
        }

        var tariff = new AdminTariff
        {
            ServerId = dto.ServerId,
            Name = dto.Name,
            Description = dto.Description,
            Flags = !string.IsNullOrWhiteSpace(dto.Flags) ? dto.Flags : null,
            GroupName = !string.IsNullOrWhiteSpace(dto.GroupName) ? dto.GroupName : null,
            Immunity = dto.Immunity,
            IsActive = dto.IsActive,
            Order = dto.Order
        };

        _context.AdminTariffs.Add(tariff);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTariffs), new { id = tariff.Id }, new AdminTariffDto
        {
            Id = tariff.Id,
            ServerId = tariff.ServerId,
            ServerName = server.Name,
            Name = tariff.Name,
            Description = tariff.Description,
            DurationDays = 0, 
            Price = 0, 
            Flags = tariff.Flags,
            GroupName = tariff.GroupName,
            Immunity = tariff.Immunity,
            IsActive = tariff.IsActive,
            Order = tariff.Order,
            CreatedAt = tariff.CreatedAt,
            Options = new List<AdminTariffOptionDto>() 
        });
    }

    [HttpPut("tariffs/{id}")]
    public async Task<ActionResult<AdminTariffDto>> UpdateTariff(int id, [FromBody] UpdateAdminTariffDto dto)
    {
        var tariff = await _context.AdminTariffs
            .Include(t => t.Server)
            .Include(t => t.Options)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        if (dto.Name != null) tariff.Name = dto.Name;
        if (dto.Description != null) tariff.Description = dto.Description;
        if (dto.Flags != null) tariff.Flags = string.IsNullOrWhiteSpace(dto.Flags) ? null : dto.Flags;
        if (dto.GroupName != null) tariff.GroupName = string.IsNullOrWhiteSpace(dto.GroupName) ? null : dto.GroupName;
        if (dto.Immunity.HasValue) tariff.Immunity = dto.Immunity.Value;
        if (dto.IsActive.HasValue) tariff.IsActive = dto.IsActive.Value;
        if (dto.Order.HasValue) tariff.Order = dto.Order.Value;

        if (!string.IsNullOrWhiteSpace(tariff.Flags) && !string.IsNullOrWhiteSpace(tariff.GroupName))
        {
            return BadRequest(new { message = "Укажите либо флаги (srv_flags), либо группу (srv_group). В SourceBans используется только один вариант." });
        }

        if (string.IsNullOrWhiteSpace(tariff.Flags) && string.IsNullOrWhiteSpace(tariff.GroupName))
        {
            return BadRequest(new { message = "Укажите либо флаги (srv_flags), либо группу (srv_group)." });
        }

        await _context.SaveChangesAsync();

        return Ok(new AdminTariffDto
        {
            Id = tariff.Id,
            ServerId = tariff.ServerId,
            ServerName = tariff.Server.Name,
            Name = tariff.Name,
            Description = tariff.Description,
            DurationDays = 0, 
            Price = 0, 
            Flags = tariff.Flags,
            GroupName = tariff.GroupName,
            Immunity = tariff.Immunity,
            IsActive = tariff.IsActive,
            Order = tariff.Order,
            CreatedAt = tariff.CreatedAt,
            Options = tariff.Options?.Select(o => new AdminTariffOptionDto
            {
                Id = o.Id,
                TariffId = o.TariffId,
                DurationDays = o.DurationDays,
                Price = o.Price,
                Order = o.Order,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            }).ToList()
        });
    }

    [HttpDelete("tariffs/{id}")]
    public async Task<IActionResult> DeleteTariff(int id)
    {
        var tariff = await _context.AdminTariffs
            .Include(t => t.Options)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        var optionIds = tariff.Options.Select(o => o.Id).ToList();

        var transactionsToUpdate = await _context.DonationTransactions
            .Where(t => t.TariffId == id || (t.TariffOptionId.HasValue && optionIds.Contains(t.TariffOptionId.Value)))
            .ToListAsync();

        foreach (var transaction in transactionsToUpdate)
        {
            transaction.TariffId = null;
            transaction.TariffOptionId = null;
        }

        var activePrivileges = await _context.UserAdminPrivileges
            .Where(p => p.TariffOptionId.HasValue && optionIds.Contains(p.TariffOptionId.Value) && p.IsActive)
            .ToListAsync();

        foreach (var privilege in activePrivileges)
        {
            privilege.IsActive = false;
            privilege.ExpiresAt = DateTimeHelper.GetServerLocalTime(); 
        }

        _context.AdminTariffs.Remove(tariff);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Тариф удален, связанные активные привилегии деактивированы" });
    }

    #endregion

    #region Donation Package

    [HttpGet("package")]
    public async Task<ActionResult<DonationPackageDto>> GetDonationPackage()
    {
        var package = await _context.DonationPackages
            .OrderByDescending(p => p.UpdatedAt)
            .FirstOrDefaultAsync();

        if (package == null)
        {
            return Ok(new DonationPackageDto { IsActive = false });
        }

        List<int>? suggestedAmounts = null;
        if (!string.IsNullOrEmpty(package.SuggestedAmounts))
        {
            try
            {
                suggestedAmounts = JsonSerializer.Deserialize<List<int>>(package.SuggestedAmounts);
            }
            catch { }
        }

        return Ok(new DonationPackageDto
        {
            Id = package.Id,
            Title = package.Title,
            Description = package.Description,
            SuggestedAmounts = suggestedAmounts,
            IsActive = package.IsActive,
            CreatedAt = package.CreatedAt,
            UpdatedAt = package.UpdatedAt
        });
    }

    [HttpPost("package")]
    public async Task<ActionResult<DonationPackageDto>> UpdateDonationPackage([FromBody] UpdateDonationPackageDto dto)
    {
        var package = await _context.DonationPackages
            .OrderByDescending(p => p.UpdatedAt)
            .FirstOrDefaultAsync();

        string? suggestedAmountsJson = null;
        if (dto.SuggestedAmounts != null && dto.SuggestedAmounts.Any())
        {
            suggestedAmountsJson = JsonSerializer.Serialize(dto.SuggestedAmounts);
        }

        if (package == null)
        {
            package = new DonationPackage
            {
                Title = dto.Title,
                Description = dto.Description,
                SuggestedAmounts = suggestedAmountsJson,
                IsActive = dto.IsActive,
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.DonationPackages.Add(package);
        }
        else
        {
            package.Title = dto.Title;
            package.Description = dto.Description;
            package.SuggestedAmounts = suggestedAmountsJson;
            package.IsActive = dto.IsActive;
            package.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();

        return Ok(new DonationPackageDto
        {
            Id = package.Id,
            Title = package.Title,
            Description = package.Description,
            SuggestedAmounts = dto.SuggestedAmounts,
            IsActive = package.IsActive,
            CreatedAt = package.CreatedAt,
            UpdatedAt = package.UpdatedAt
        });
    }

    #endregion

    #region Transactions

    [HttpGet("transactions")]
    public async Task<ActionResult<List<DonationTransactionDto>>> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = _context.DonationTransactions
            .Include(t => t.User)
            .Include(t => t.Tariff)
            .Include(t => t.Server)
            .OrderByDescending(t => t.CreatedAt);

        var transactions = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = transactions.Select(t => new DonationTransactionDto
        {
            Id = t.Id,
            TransactionId = t.TransactionId,
            UserId = t.UserId,
            Username = t.User?.Username,
            SteamId = t.SteamId,
            Amount = t.Amount,
            Type = t.Type,
            TariffId = t.TariffId,
            TariffName = t.Tariff?.Name,
            ServerId = t.ServerId,
            ServerName = t.Server?.Name,
            Status = t.Status,
            ExpiresAt = t.ExpiresAt,
            CreatedAt = t.CreatedAt,
            CompletedAt = t.CompletedAt
        }).ToList();

        return Ok(result);
    }

    #endregion

    #region User Privileges

    [HttpGet("privileges")]
    public async Task<ActionResult<List<object>>> GetAllPrivileges([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = _context.UserAdminPrivileges
            .Include(p => p.User)
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .OrderByDescending(p => p.IsActive)
            .ThenByDescending(p => p.CreatedAt);

        var privileges = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = privileges.Select(p => new
        {
            p.Id,
            userId = p.UserId,
            username = p.User?.Username ?? "Неизвестный пользователь",
            p.SteamId,
            serverId = p.ServerId,
            serverName = p.Server?.Name ?? "Неизвестный сервер",
            tariffName = p.Tariff?.Name ?? "Неизвестный тариф",
            p.Flags,
            p.GroupName,
            p.Immunity,
            p.StartsAt,
            p.ExpiresAt,
            isActive = p.IsActive && p.ExpiresAt > now,
            isExpired = p.ExpiresAt <= now,
            daysRemaining = p.ExpiresAt > now ? (int)(p.ExpiresAt - now).TotalDays : 0,
            p.CreatedAt
        }).ToList();

        return Ok(result);
    }

    #endregion

    #region Tariff Options

    [HttpGet("tariffs/{tariffId}/options")]
    public async Task<ActionResult<List<AdminTariffOptionDto>>> GetTariffOptions(int tariffId)
    {
        var options = await _context.AdminTariffOptions
            .Where(o => o.TariffId == tariffId)
            .OrderBy(o => o.Order)
            .ToListAsync();

        var result = options.Select(o => new AdminTariffOptionDto
        {
            Id = o.Id,
            TariffId = o.TariffId,
            DurationDays = o.DurationDays,
            Price = o.Price,
            Order = o.Order,
            IsActive = o.IsActive,
            CreatedAt = o.CreatedAt
        }).ToList();

        return Ok(result);
    }

    [HttpPost("tariffs/{tariffId}/options")]
    public async Task<ActionResult<AdminTariffOptionDto>> CreateTariffOption(int tariffId, [FromBody] CreateAdminTariffOptionDto dto)
    {
        var tariff = await _context.AdminTariffs.FindAsync(tariffId);
        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        var existingOption = await _context.AdminTariffOptions
            .FirstOrDefaultAsync(o => o.TariffId == tariffId && o.DurationDays == dto.DurationDays && o.Price == dto.Price);

        if (existingOption != null)
        {
            return BadRequest(new { message = "Вариант с такими параметрами уже существует для этого тарифа" });
        }

        var option = new AdminTariffOption
        {
            TariffId = tariffId,
            DurationDays = dto.DurationDays,
            Price = dto.Price,
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        _context.AdminTariffOptions.Add(option);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTariffOptions), new { tariffId }, new AdminTariffOptionDto
        {
            Id = option.Id,
            TariffId = option.TariffId,
            DurationDays = option.DurationDays,
            Price = option.Price,
            Order = option.Order,
            IsActive = option.IsActive,
            CreatedAt = option.CreatedAt
        });
    }

    [HttpPut("tariffs/{tariffId}/options/{optionId}")]
    public async Task<ActionResult<AdminTariffOptionDto>> UpdateTariffOption(int tariffId, int optionId, [FromBody] UpdateAdminTariffOptionDto dto)
    {
        var option = await _context.AdminTariffOptions
            .FirstOrDefaultAsync(o => o.Id == optionId && o.TariffId == tariffId);

        if (option == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        if (dto.DurationDays.HasValue) option.DurationDays = dto.DurationDays.Value;
        if (dto.Price.HasValue) option.Price = dto.Price.Value;
        if (dto.Order.HasValue) option.Order = dto.Order.Value;
        if (dto.IsActive.HasValue) option.IsActive = dto.IsActive.Value;

        var duplicateOption = await _context.AdminTariffOptions
            .FirstOrDefaultAsync(o => o.TariffId == tariffId && o.DurationDays == option.DurationDays && o.Price == option.Price && o.Id != optionId);

        if (duplicateOption != null)
        {
            return BadRequest(new { message = "Вариант с такими параметрами уже существует для этого тарифа" });
        }

        await _context.SaveChangesAsync();

        return Ok(new AdminTariffOptionDto
        {
            Id = option.Id,
            TariffId = option.TariffId,
            DurationDays = option.DurationDays,
            Price = option.Price,
            Order = option.Order,
            IsActive = option.IsActive,
            CreatedAt = option.CreatedAt
        });
    }

    [HttpDelete("tariffs/{tariffId}/options/{optionId}")]
    public async Task<IActionResult> DeleteTariffOption(int tariffId, int optionId)
    {
        var option = await _context.AdminTariffOptions
            .FirstOrDefaultAsync(o => o.Id == optionId && o.TariffId == tariffId);

        if (option == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        var hasTransactions = await _context.DonationTransactions
            .AnyAsync(t => t.TariffOptionId == optionId);

        if (hasTransactions)
        {
            return BadRequest(new { message = "Нельзя удалить вариант, который используется в транзакциях" });
        }

        var hasActivePrivileges = await _context.UserAdminPrivileges
            .AnyAsync(p => p.TariffOptionId == optionId && p.IsActive);

        if (hasActivePrivileges)
        {
            return BadRequest(new { message = "Нельзя удалить вариант, который используется в активных админ-привилегиях" });
        }

        _context.AdminTariffOptions.Remove(option);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Вариант тарифа удален" });
    }

    #endregion

    #region Transaction Approval

    [HttpPost("transactions/{transactionId}/approve")]
    public async Task<ActionResult> ApproveTransaction(string transactionId)
    {
        var transaction = await _context.DonationTransactions
            .Include(t => t.User)
            .Include(t => t.Server)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

        if (transaction == null)
        {
            return NotFound(new { message = "Транзакция не найдена" });
        }

        if (transaction.Status != "pending")
        {
            return BadRequest(new { message = $"Транзакция уже обработана. Статус: {transaction.Status}" });
        }

        if (transaction.Type != "admin_purchase" && transaction.Type != "vip_purchase")
        {
            return BadRequest(new { message = "Можно подтверждать только платежи за админку или VIP" });
        }

        if (string.IsNullOrEmpty(transaction.SteamId))
        {
            return BadRequest(new { message = "У пользователя не указан Steam ID" });
        }

        try
        {
            if (transaction.Type == "vip_purchase")
            {
                var vipTariffOption = await _context.VipTariffOptions
                    .Include(o => o.Tariff)
                    .FirstOrDefaultAsync(o => o.Id == transaction.VipTariffOptionId);

                if (vipTariffOption == null || vipTariffOption.Tariff.ServerId != transaction.ServerId)
                {
                    return BadRequest(new { message = "Недостаточно данных для выдачи VIP" });
                }

                var durationSeconds = vipTariffOption.DurationDays == 0
                    ? 0 
                    : vipTariffOption.DurationDays * 24 * 60 * 60; 

                var vipService = HttpContext.RequestServices.GetRequiredService<IVipService>();
                var success = await vipService.AddVipAsync(
                    steamId: transaction.SteamId,
                    name: transaction.User?.Username ?? "Unknown",
                    groupName: vipTariffOption.Tariff.GroupName,
                    durationSeconds: durationSeconds
                );

                if (!success)
                {
                    return StatusCode(500, new { message = "Не удалось добавить VIP в базу данных" });
                }

                var privilege = new UserVipPrivilege
                {
                    UserId = transaction.UserId ?? 0,
                    SteamId = transaction.SteamId,
                    ServerId = transaction.ServerId ?? 0,
                    TariffId = vipTariffOption.TariffId,
                    TariffOptionId = vipTariffOption.Id,
                    GroupName = vipTariffOption.Tariff.GroupName,
                    StartsAt = DateTimeHelper.GetServerLocalTime(),
                    ExpiresAt = transaction.ExpiresAt ?? DateTime.MaxValue,
                    IsActive = true,
                    TransactionId = transaction.Id
                };

                _context.UserVipPrivileges.Add(privilege);

                transaction.Status = "completed";
                transaction.CompletedAt = DateTimeHelper.GetServerLocalTime();

                await _context.SaveChangesAsync();

                if (transaction.User != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await SendVipPurchaseConfirmationEmailAsync(transaction, privilege);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send VIP purchase confirmation email for transaction {TransactionId}", transactionId);
                        }
                    });
                }

                return Ok(new
                {
                    message = "Платеж подтвержден, VIP статус активирован",
                    transactionId = transaction.TransactionId,
                    privilegeId = privilege.Id,
                    serverName = transaction.Server?.Name ?? "Неизвестный сервер",
                    tariffName = vipTariffOption.Tariff.Name,
                    vipGroup = vipTariffOption.Tariff.GroupName,
                    expiresAt = transaction.ExpiresAt
                });
            }
            else 
            {
                var adminTariffOption = await _context.AdminTariffOptions
                    .Include(o => o.Tariff)
                    .FirstOrDefaultAsync(o => o.Id == transaction.TariffOptionId);

                if (adminTariffOption == null || adminTariffOption.Tariff.ServerId != transaction.ServerId)
                {
                    return BadRequest(new { message = "Недостаточно данных для выдачи админки" });
                }

                var durationSeconds = adminTariffOption.DurationDays == 0
                    ? 0 
                    : adminTariffOption.DurationDays * 24 * 60 * 60; 

                var adminId = await _sourceBansService.AddAdminAsync(
                    steamId: transaction.SteamId,
                    name: transaction.User?.Username ?? "Unknown",
                    flags: adminTariffOption.Tariff.Flags,
                    groupName: adminTariffOption.Tariff.GroupName,
                    immunity: adminTariffOption.Tariff.Immunity,
                    durationSeconds: durationSeconds,
                    password: transaction.AdminPassword
                );

                if (adminId == null)
                {
                    return StatusCode(500, new { message = "Не удалось добавить админа в SourceBans базу данных" });
                }

                var privilege = new UserAdminPrivilege
                {
                    UserId = transaction.UserId ?? 0,
                    SteamId = transaction.SteamId,
                    ServerId = transaction.ServerId ?? 0,
                    TariffId = adminTariffOption.TariffId,
                    TariffOptionId = adminTariffOption.Id,
                    Flags = adminTariffOption.Tariff.Flags,
                    GroupName = adminTariffOption.Tariff.GroupName,
                    Immunity = adminTariffOption.Tariff.Immunity,
                    StartsAt = DateTimeHelper.GetServerLocalTime(),
                    ExpiresAt = transaction.ExpiresAt ?? DateTime.MaxValue,
                    IsActive = true,
                    AdminPassword = transaction.AdminPassword,
                    SourceBansAdminId = adminId.Value,
                    TransactionId = transaction.Id
                };

                _context.UserAdminPrivileges.Add(privilege);

                transaction.Status = "completed";
                transaction.CompletedAt = DateTimeHelper.GetServerLocalTime();

                await _context.SaveChangesAsync();

                if (transaction.User != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await SendAdminPurchaseConfirmationEmailAsync(transaction, privilege);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send admin purchase confirmation email for transaction {TransactionId}", transactionId);
                        }
                    });
                }

                return Ok(new
                {
                    message = "Платеж подтвержден, админка выдана",
                    transactionId = transaction.TransactionId,
                    adminId = adminId.Value,
                    privilegeId = privilege.Id,
                    serverName = transaction.Server?.Name ?? "Неизвестный сервер",
                    tariffName = adminTariffOption.Tariff.Name,
                    expiresAt = transaction.ExpiresAt
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving transaction {TransactionId}", transactionId);
            return StatusCode(500, new { message = "Ошибка при подтверждении платежа" });
        }
    }

    private async Task SendAdminPurchaseConfirmationEmailAsync(DonationTransaction transaction, UserAdminPrivilege privilege)
    {
        try
        {
            var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
            
            await emailService.SendAdminPurchaseConfirmationAsync(
                toEmail: transaction.User!.Email,
                username: transaction.User.Username,
                serverName: transaction.Server!.Name,
                adminPassword: transaction.AdminPassword ?? "",
                expiresAt: privilege.ExpiresAt
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send admin purchase confirmation email for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    private async Task SendVipPurchaseConfirmationEmailAsync(DonationTransaction transaction, UserVipPrivilege privilege)
    {
        try
        {
            var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
            
            await emailService.SendVipPurchaseConfirmationAsync(
                toEmail: transaction.User!.Email,
                username: transaction.User.Username,
                serverName: transaction.Server!.Name,
                vipGroup: privilege.GroupName,
                expiresAt: privilege.ExpiresAt
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send VIP purchase confirmation email for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    #endregion

    #region Transaction Rejection

    [HttpPost("transactions/{transactionId}/reject")]
    public async Task<ActionResult> RejectTransaction(string transactionId)
    {
        var transaction = await _context.DonationTransactions
            .Include(t => t.User)
            .Include(t => t.Tariff)
            .Include(t => t.Server)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

        if (transaction == null)
        {
            return NotFound(new { message = "Транзакция не найдена" });
        }

        if (transaction.Status != "pending")
        {
            return BadRequest(new { message = $"Транзакция уже обработана. Статус: {transaction.Status}" });
        }

        try
        {
            transaction.Status = "cancelled";
            transaction.CancelledAt = DateTimeHelper.GetServerLocalTime();

            await _context.SaveChangesAsync();

            if (transaction.User != null)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await SendTransactionRejectionEmailAsync(transaction);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send transaction rejection email for transaction {TransactionId}", transactionId);
                    }
                });
            }

            return Ok(new
            {
                message = "Платеж отклонен",
                transactionId = transaction.TransactionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting transaction {TransactionId}", transactionId);
            return StatusCode(500, new { message = "Ошибка при отклонении платежа" });
        }
    }

    private async Task SendTransactionRejectionEmailAsync(DonationTransaction transaction)
    {
        try
        {
            var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
            var subject = $"Платеж отклонен - {transaction.TransactionId}";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #dc3545;'>Платеж отклонен</h2>
                        <p>Привет, {transaction.User!.Username}!</p>
                        <p>Ваш платеж <strong>{transaction.TransactionId}</strong> был отклонен администратором.</p>
                        <p>Если у вас есть вопросы, пожалуйста, обратитесь в поддержку.</p>
                        <p style='margin-top: 30px;'>
                            <a href='{_frontendUrl}' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Вернуться на сайт
                            </a>
                        </p>
                    </div>
                </body>
                </html>
            ";

            var smtpSettings = await _context.SmtpSettings
                .Where(s => s.IsConfigured)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            if (smtpSettings != null)
            {
                var mailService = new System.Net.Mail.SmtpClient(smtpSettings.Host, smtpSettings.Port)
                {
                    Credentials = new System.Net.NetworkCredential(smtpSettings.Username, smtpSettings.Password),
                    EnableSsl = smtpSettings.EnableSsl
                };

                var message = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress(smtpSettings.FromEmail, smtpSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(transaction.User.Email);

                await mailService.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send transaction rejection email for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    #endregion

    #region VIP Settings (per server)

    [HttpGet("vip-settings")]
    public async Task<ActionResult> GetAllVipSettings()
    {
        var settings = await _context.VipSettings
            .Include(s => s.Server)
            .OrderBy(s => s.Server.Name)
            .Select(s => new
            {
                s.Id,
                s.ServerId,
                serverName = s.Server.Name,
                s.Host,
                s.Port,
                s.Database,
                s.Username,
                s.Password,
                s.IsConfigured,
                s.CreatedAt,
                s.UpdatedAt
            })
            .ToListAsync();

        return Ok(settings);
    }

    [HttpGet("vip-settings/{serverId}")]
    public async Task<ActionResult> GetVipSettingsByServer(int serverId)
    {
        var settings = await _context.VipSettings
            .Include(s => s.Server)
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return Ok(new { isConfigured = false, serverId });
        }

        return Ok(new
        {
            settings.Id,
            settings.ServerId,
            serverName = settings.Server.Name,
            settings.Host,
            settings.Port,
            settings.Database,
            settings.Username,
            settings.Password,
            settings.IsConfigured,
            settings.CreatedAt,
            settings.UpdatedAt
        });
    }

    [HttpPost("vip-settings")]
    public async Task<ActionResult> UpsertVipSettings([FromBody] UpdateVipSettingsDto dto)
    {
        if (!dto.ServerId.HasValue)
        {
            return BadRequest(new { message = "ServerId обязателен" });
        }

        var server = await _context.Servers.FindAsync(dto.ServerId.Value);
        if (server == null)
        {
            return NotFound(new { message = "Сервер не найден" });
        }

        var settings = await _context.VipSettings
            .FirstOrDefaultAsync(s => s.ServerId == dto.ServerId.Value);

        if (settings == null)
        {
            settings = new VipSettings
            {
                ServerId = dto.ServerId.Value,
                Host = dto.Host,
                Port = dto.Port,
                Database = dto.Database,
                Username = dto.Username,
                Password = dto.Password,
                IsConfigured = true,
                CreatedAt = DateTimeHelper.GetServerLocalTime(),
                UpdatedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.VipSettings.Add(settings);
        }
        else
        {
            settings.Host = dto.Host;
            settings.Port = dto.Port;
            settings.Database = dto.Database;
            settings.Username = dto.Username;
            settings.Password = dto.Password;
            settings.IsConfigured = true;
            settings.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            settings.Id,
            settings.ServerId,
            serverName = server.Name,
            settings.Host,
            settings.Port,
            settings.Database,
            settings.Username,
            settings.Password,
            settings.IsConfigured,
            settings.CreatedAt,
            settings.UpdatedAt
        });
    }

    [HttpDelete("vip-settings/{serverId}")]
    public async Task<ActionResult> DeleteVipSettings(int serverId)
    {
        var settings = await _context.VipSettings
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return NotFound(new { message = "Настройки не найдены" });
        }

        _context.VipSettings.Remove(settings);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Настройки удалены" });
    }

    [HttpPost("vip-settings/{serverId}/test")]
    public async Task<ActionResult> TestVipConnection(int serverId)
    {
        var settings = await _context.VipSettings
            .FirstOrDefaultAsync(s => s.ServerId == serverId);

        if (settings == null)
        {
            return NotFound(new { success = false, message = "Настройки VIP для этого сервера не найдены" });
        }

        var vipService = HttpContext.RequestServices.GetRequiredService<IVipService>();
        var success = await vipService.TestConnectionAsync(serverId);

        if (success)
        {
            return Ok(new { success = true, message = "Подключение успешно! Таблица vip_users найдена." });
        }
        else
        {
            return Ok(new { success = false, message = "Ошибка подключения к VIP базе данных" });
        }
    }

    #endregion

    #region VIP Tariffs

    [HttpGet("servers-with-vip")]
    public async Task<ActionResult<List<object>>> GetServersWithVip()
    {
        var serversWithVip = await _context.VipSettings
            .Where(s => s.IsConfigured)
            .Include(s => s.Server)
            .Select(s => new
            {
                id = s.ServerId,
                name = s.Server.Name,
                ipAddress = $"{s.Server.IpAddress}:{s.Server.Port}"
            })
            .ToListAsync();

        return Ok(serversWithVip);
    }

    [HttpGet("vip-tariffs")]
    public async Task<ActionResult<List<VipTariffDto>>> GetVipTariffs()
    {
        var tariffs = await _context.VipTariffs
            .Include(t => t.Server)
            .Include(t => t.Options)
            .OrderBy(t => t.ServerId)
            .ThenBy(t => t.Order)
            .ToListAsync();

        var result = tariffs.Select(t => new VipTariffDto
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
            Options = t.Options?.Select(o => new VipTariffOptionDto
            {
                Id = o.Id,
                TariffId = o.TariffId,
                DurationDays = o.DurationDays,
                Price = o.Price,
                Order = o.Order,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    [HttpPost("vip-tariffs")]
    public async Task<ActionResult<VipTariffDto>> CreateVipTariff([FromBody] CreateVipTariffDto dto)
    {
        var server = await _context.Servers.FindAsync(dto.ServerId);
        if (server == null)
        {
            return NotFound(new { message = "Сервер не найден" });
        }

        var vipSettings = await _context.VipSettings
            .FirstOrDefaultAsync(s => s.ServerId == dto.ServerId && s.IsConfigured);

        if (vipSettings == null)
        {
            return BadRequest(new { message = "Для этого сервера не настроен VIP. Сначала настройте подключение к базе данных." });
        }

        if (string.IsNullOrWhiteSpace(dto.GroupName))
        {
            return BadRequest(new { message = "Укажите группу VIP (group_name)." });
        }

        var tariff = new VipTariff
        {
            ServerId = dto.ServerId,
            Name = dto.Name,
            Description = dto.Description,
            GroupName = dto.GroupName,
            IsActive = dto.IsActive,
            Order = dto.Order
        };

        _context.VipTariffs.Add(tariff);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVipTariffs), new { id = tariff.Id }, new VipTariffDto
        {
            Id = tariff.Id,
            ServerId = tariff.ServerId,
            ServerName = server.Name,
            Name = tariff.Name,
            Description = tariff.Description,
            GroupName = tariff.GroupName,
            IsActive = tariff.IsActive,
            Order = tariff.Order,
            CreatedAt = tariff.CreatedAt,
            Options = new List<VipTariffOptionDto>() 
        });
    }

    [HttpPut("vip-tariffs/{id}")]
    public async Task<ActionResult<VipTariffDto>> UpdateVipTariff(int id, [FromBody] UpdateVipTariffDto dto)
    {
        var tariff = await _context.VipTariffs
            .Include(t => t.Server)
            .Include(t => t.Options)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        if (dto.Name != null) tariff.Name = dto.Name;
        if (dto.Description != null) tariff.Description = dto.Description;
        if (dto.GroupName != null) tariff.GroupName = string.IsNullOrWhiteSpace(dto.GroupName) ? string.Empty : dto.GroupName;
        if (dto.IsActive.HasValue) tariff.IsActive = dto.IsActive.Value;
        if (dto.Order.HasValue) tariff.Order = dto.Order.Value;

        if (string.IsNullOrWhiteSpace(tariff.GroupName))
        {
            return BadRequest(new { message = "Укажите группу VIP (group_name)." });
        }

        await _context.SaveChangesAsync();

        return Ok(new VipTariffDto
        {
            Id = tariff.Id,
            ServerId = tariff.ServerId,
            ServerName = tariff.Server.Name,
            Name = tariff.Name,
            Description = tariff.Description,
            GroupName = tariff.GroupName ?? string.Empty,
            IsActive = tariff.IsActive,
            Order = tariff.Order,
            CreatedAt = tariff.CreatedAt,
            Options = tariff.Options?.Select(o => new VipTariffOptionDto
            {
                Id = o.Id,
                TariffId = o.TariffId,
                DurationDays = o.DurationDays,
                Price = o.Price,
                Order = o.Order,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt
            }).ToList()
        });
    }

    [HttpDelete("vip-tariffs/{id}")]
    public async Task<IActionResult> DeleteVipTariff(int id)
    {
        var tariff = await _context.VipTariffs
            .Include(t => t.Options)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        var optionIds = tariff.Options.Select(o => o.Id).ToList();

        var transactionsToUpdate = await _context.DonationTransactions
            .Where(t => t.TariffId == id || t.VipTariffId == id || 
                       (t.TariffOptionId.HasValue && optionIds.Contains(t.TariffOptionId.Value)) ||
                       (t.VipTariffOptionId.HasValue && optionIds.Contains(t.VipTariffOptionId.Value)))
            .ToListAsync();

        foreach (var transaction in transactionsToUpdate)
        {
            transaction.TariffId = null;
            transaction.TariffOptionId = null;
            transaction.VipTariffId = null;
            transaction.VipTariffOptionId = null;
        }

        var activePrivileges = await _context.UserVipPrivileges
            .Where(p => p.TariffOptionId.HasValue && optionIds.Contains(p.TariffOptionId.Value) && p.IsActive)
            .ToListAsync();

        foreach (var privilege in activePrivileges)
        {
            privilege.IsActive = false;
            privilege.ExpiresAt = DateTimeHelper.GetServerLocalTime();
        }

        _context.VipTariffs.Remove(tariff);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Тариф удален, связанные активные привилегии деактивированы" });
    }

    #endregion

    #region VIP Tariff Options

    [HttpGet("vip-tariffs/{tariffId}/options")]
    public async Task<ActionResult<List<VipTariffOptionDto>>> GetVipTariffOptions(int tariffId)
    {
        var options = await _context.VipTariffOptions
            .Where(o => o.TariffId == tariffId)
            .OrderBy(o => o.Order)
            .ToListAsync();

        var result = options.Select(o => new VipTariffOptionDto
        {
            Id = o.Id,
            TariffId = o.TariffId,
            DurationDays = o.DurationDays,
            Price = o.Price,
            Order = o.Order,
            IsActive = o.IsActive,
            CreatedAt = o.CreatedAt
        }).ToList();

        return Ok(result);
    }

    [HttpPost("vip-tariffs/{tariffId}/options")]
    public async Task<ActionResult<VipTariffOptionDto>> CreateVipTariffOption(int tariffId, [FromBody] CreateVipTariffOptionDto dto)
    {
        var tariff = await _context.VipTariffs.FindAsync(tariffId);
        if (tariff == null)
        {
            return NotFound(new { message = "Тариф не найден" });
        }

        var existingOption = await _context.VipTariffOptions
            .FirstOrDefaultAsync(o => o.TariffId == tariffId && o.DurationDays == dto.DurationDays && o.Price == dto.Price);

        if (existingOption != null)
        {
            return BadRequest(new { message = "Вариант с такими параметрами уже существует для этого тарифа" });
        }

        var option = new VipTariffOption
        {
            TariffId = tariffId,
            DurationDays = dto.DurationDays,
            Price = dto.Price,
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        _context.VipTariffOptions.Add(option);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVipTariffOptions), new { tariffId }, new VipTariffOptionDto
        {
            Id = option.Id,
            TariffId = option.TariffId,
            DurationDays = option.DurationDays,
            Price = option.Price,
            Order = option.Order,
            IsActive = option.IsActive,
            CreatedAt = option.CreatedAt
        });
    }

    [HttpPut("vip-tariffs/{tariffId}/options/{optionId}")]
    public async Task<ActionResult<VipTariffOptionDto>> UpdateVipTariffOption(int tariffId, int optionId, [FromBody] UpdateVipTariffOptionDto dto)
    {
        var option = await _context.VipTariffOptions
            .FirstOrDefaultAsync(o => o.Id == optionId && o.TariffId == tariffId);

        if (option == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        if (dto.DurationDays.HasValue) option.DurationDays = dto.DurationDays.Value;
        if (dto.Price.HasValue) option.Price = dto.Price.Value;
        if (dto.Order.HasValue) option.Order = dto.Order.Value;
        if (dto.IsActive.HasValue) option.IsActive = dto.IsActive.Value;

        var duplicateOption = await _context.VipTariffOptions
            .FirstOrDefaultAsync(o => o.TariffId == tariffId && o.DurationDays == option.DurationDays && o.Price == option.Price && o.Id != optionId);

        if (duplicateOption != null)
        {
            return BadRequest(new { message = "Вариант с такими параметрами уже существует для этого тарифа" });
        }

        await _context.SaveChangesAsync();

        return Ok(new VipTariffOptionDto
        {
            Id = option.Id,
            TariffId = option.TariffId,
            DurationDays = option.DurationDays,
            Price = option.Price,
            Order = option.Order,
            IsActive = option.IsActive,
            CreatedAt = option.CreatedAt
        });
    }

    [HttpDelete("vip-tariffs/{tariffId}/options/{optionId}")]
    public async Task<IActionResult> DeleteVipTariffOption(int tariffId, int optionId)
    {
        var option = await _context.VipTariffOptions
            .FirstOrDefaultAsync(o => o.Id == optionId && o.TariffId == tariffId);

        if (option == null)
        {
            return NotFound(new { message = "Вариант тарифа не найден" });
        }

        var transactionsToUpdate = await _context.DonationTransactions
            .Where(t => t.VipTariffOptionId == optionId)
            .ToListAsync();

        foreach (var transaction in transactionsToUpdate)
        {
            transaction.VipTariffOptionId = null;
        }

        var activePrivileges = await _context.UserVipPrivileges
            .Where(p => p.TariffOptionId == optionId && p.IsActive)
            .ToListAsync();

        foreach (var privilege in activePrivileges)
        {
            privilege.IsActive = false;
            privilege.ExpiresAt = DateTimeHelper.GetServerLocalTime();
        }

        _context.VipTariffOptions.Remove(option);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Вариант тарифа удален, связанные активные привилегии деактивированы" });
    }

    #endregion

    #region VIP Privileges

    [HttpGet("vip-privileges")]
    public async Task<ActionResult<List<object>>> GetAllVipPrivileges([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = _context.UserVipPrivileges
            .Include(p => p.User)
            .Include(p => p.Server)
            .Include(p => p.Tariff)
            .OrderByDescending(p => p.IsActive)
            .ThenByDescending(p => p.CreatedAt);

        var privileges = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var now = DateTimeHelper.GetServerLocalTime();
        var result = privileges.Select(p => new
        {
            p.Id,
            userId = p.UserId,
            username = p.User?.Username ?? "Неизвестный пользователь",
            p.SteamId,
            serverId = p.ServerId,
            serverName = p.Server?.Name ?? "Неизвестный сервер",
            tariffName = p.Tariff?.Name ?? "Неизвестный тариф",
            p.GroupName,
            p.StartsAt,
            p.ExpiresAt,
            isActive = p.IsActive && p.ExpiresAt > now,
            isExpired = p.ExpiresAt <= now,
            daysRemaining = p.ExpiresAt > now ? (int)(p.ExpiresAt - now).TotalDays : 0,
            p.CreatedAt
        }).ToList();

        return Ok(result);
    }

    [HttpDelete("vip-privileges/{privilegeId}")]
    public async Task<IActionResult> RemoveVipPrivilege(int privilegeId)
    {
        try
        {
            var vipSyncService = HttpContext.RequestServices.GetRequiredService<IVipSyncService>();
            var result = await vipSyncService.RemoveVipAsync(privilegeId);

            if (!result)
            {
                return NotFound(new { message = "VIP привилегия не найдена" });
            }

            return Ok(new { message = "VIP привилегия успешно удалена" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing VIP privilege {PrivilegeId}", privilegeId);
            return StatusCode(500, new { message = "Ошибка при удалении VIP привилегии", error = ex.Message });
        }
    }

    #region VIP Applications

    [HttpGet("vip-applications")]
    public async Task<ActionResult<List<VipApplicationDto>>> GetVipApplications([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = _context.VipApplications
            .Include(a => a.User)
            .Include(a => a.Server)
            .OrderByDescending(a => a.CreatedAt);

        var total = await query.CountAsync();
        List<Models.VipApplication> apps;
        try
        {
            apps = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        catch (MySqlConnector.MySqlException mex) when (mex.Message?.Contains("Unknown column 'v.duration_days'") == true || mex.Message?.Contains("Unknown column 'vip_applications.duration_days'") == true)
        {
            var proj = await _context.VipApplications
                .Include(a => a.User)
                .Include(a => a.Server)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new
                {
                    a.Id,
                    a.UserId,
                    a.Username,
                    a.SteamId,
                    a.ServerId,
                    serverName = a.Server != null ? a.Server.Name : "Неизвестный сервер",
                    a.HoursPerWeek,
                    a.Reason,
                    a.Status,
                    a.AdminId,
                    a.AdminComment,
                    VipGroup = a.VipGroup,
                    DurationDays = (int?)null,
                    a.CreatedAt,
                    a.ProcessedAt
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            apps = proj.Select(p => new Models.VipApplication
            {
                Id = p.Id,
                UserId = p.UserId,
                Username = p.Username,
                SteamId = p.SteamId,
                ServerId = p.ServerId,
                Server = new Models.Server { Id = p.ServerId, Name = p.serverName },
                HoursPerWeek = p.HoursPerWeek,
                Reason = p.Reason,
                Status = p.Status,
                AdminId = p.AdminId,
                AdminComment = p.AdminComment,
                VipGroup = p.VipGroup,
                DurationDays = null,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt
            }).ToList();
        }

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
            AdminId = a.AdminId,
            AdminComment = a.AdminComment,
            VipGroup = a.VipGroup,
            DurationDays = a.DurationDays,
            CreatedAt = a.CreatedAt,
            ProcessedAt = a.ProcessedAt
        }).ToList();

        return Ok(new { items = result, total, page, pageSize });
    }

    [HttpPost("vip-applications/{id}/approve")]
    public async Task<IActionResult> ApproveVipApplication(int id, [FromBody] AdminApproveVipApplicationDto dto)
    {
        try
        {
            var application = await GetVipApplicationSafely(id);
            if (application == null) return NotFound(new { message = "Заявка не найдена" });

            if (application.Status != "pending") return BadRequest(new { message = "Заявка уже обработана" });

            var vipService = HttpContext.RequestServices.GetRequiredService<IVipService>();
            var notificationService = HttpContext.RequestServices.GetRequiredService<INotificationService>();

            if (!dto.TariffOptionId.HasValue)
            {
                return BadRequest(new { message = "Для одобрения заявки необходимо выбрать опцию VIP тарифа на сервере." });
            }

            var vipTariffOption = await _context.VipTariffOptions
                .Include(o => o.Tariff)
                .FirstOrDefaultAsync(o => o.Id == dto.TariffOptionId.Value);

            if (vipTariffOption == null || vipTariffOption.Tariff == null || vipTariffOption.Tariff.ServerId != application.ServerId)
            {
                return BadRequest(new { message = "Выбранная опция тарифа не найдена или не принадлежит указанному серверу." });
            }

            if (!vipTariffOption.IsActive || !vipTariffOption.Tariff.IsActive)
            {
                _logger.LogInformation("Approving VIP application {ApplicationId} with inactive tariff/option (tariffId={TariffId}, optionId={OptionId}, tariffIsActive={TariffActive}, optionIsActive={OptionActive})", id, vipTariffOption.TariffId, vipTariffOption.Id, vipTariffOption.Tariff.IsActive, vipTariffOption.IsActive);
            }

            var durationSeconds = vipTariffOption.DurationDays == 0 ? 0 : vipTariffOption.DurationDays * 24 * 60 * 60;
            var success = await vipService.AddVipAsync(application.SteamId, application.Username, vipTariffOption.Tariff.GroupName, durationSeconds);

            if (!success)
            {
                return StatusCode(500, new { message = "Не удалось добавить VIP во внешней базе" });
            }

            int? resolvedTariffId = vipTariffOption.TariffId;
            int? resolvedTariffOptionId = vipTariffOption.Id;

            var expiry = vipTariffOption.DurationDays == 0 ? DateTime.MaxValue : DateTimeHelper.GetServerLocalTime().AddDays(vipTariffOption.DurationDays);

            var privilege = new UserVipPrivilege
            {
                UserId = application.UserId,
                SteamId = application.SteamId,
                ServerId = application.ServerId,
                TariffId = resolvedTariffId,
                TariffOptionId = resolvedTariffOptionId,
                GroupName = dto.VipGroup,
                StartsAt = DateTimeHelper.GetServerLocalTime(),
                ExpiresAt = expiry,
                IsActive = expiry > DateTimeHelper.GetServerLocalTime()
            };

            _context.UserVipPrivileges.Add(privilege);

            var adminId = GetCurrentUserId();
            var now = DateTimeHelper.GetServerLocalTime();
            var hasDurationColumn = await TableHasColumnAsync("vip_applications", "duration_days");

            if (hasDurationColumn)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE vip_applications
                    SET status = {"approved"}, admin_id = {adminId}, vip_group = {vipTariffOption.Tariff.GroupName}, duration_days = {vipTariffOption.DurationDays}, processed_at = {now}, updated_at = {now}
                    WHERE id = {id}");
            }
            else
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE vip_applications
                    SET status = {"approved"}, admin_id = {adminId}, vip_group = {vipTariffOption.Tariff.GroupName}, processed_at = {now}, updated_at = {now}
                    WHERE id = {id}");
            }

            await _context.SaveChangesAsync();

            var title = "Ваша заявка на участника одобрена";
            var message = $"Ваша заявка на участника на сервере {application.Server?.Name ?? "Неизвестный сервер"} была одобрена — выданы группа {dto.VipGroup} на { (dto.DurationDays == 0 ? "навсегда" : dto.DurationDays + " дней") }";
            await notificationService.CreateNotificationAsync(application.UserId, title, message, "vip_application_result", application.Id);

            return Ok(new { message = "Заявка одобрена и VIP выдан" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving VIP application {ApplicationId}", id);
            return StatusCode(500, new { message = "Ошибка при одобрении заявки", error = ex.Message });
        }
    }

    [HttpPost("vip-applications/{id}/reject")]
    public async Task<IActionResult> RejectVipApplication(int id, [FromBody] AdminRejectVipApplicationDto dto)
    {
        try
        {
            var application = await GetVipApplicationSafely(id);
            if (application == null) return NotFound(new { message = "Заявка не найдена" });

            if (application.Status != "pending") return BadRequest(new { message = "Заявка уже обработана" });

            var rejAdminId = GetCurrentUserId();
            var rejNow = DateTimeHelper.GetServerLocalTime();
            var rejHasDurationColumn = await TableHasColumnAsync("vip_applications", "duration_days");

            if (rejHasDurationColumn)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE vip_applications
                    SET status = {"rejected"}, admin_id = {rejAdminId}, admin_comment = {dto.Reason}, processed_at = {rejNow}, updated_at = {rejNow}
                    WHERE id = {id}");
            }
            else
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE vip_applications
                    SET status = {"rejected"}, admin_id = {rejAdminId}, admin_comment = {dto.Reason}, processed_at = {rejNow}, updated_at = {rejNow}
                    WHERE id = {id}");
            }

            await _context.SaveChangesAsync();

            var notificationService = HttpContext.RequestServices.GetRequiredService<INotificationService>();
            var title = "Ваша заявка на участника отклонена";
            var message = $"Ваша заявка на участника была отклонена. Причина: {dto.Reason}";
            await notificationService.CreateNotificationAsync(application.UserId, title, message, "vip_application_result", application.Id);

            return Ok(new { message = "Заявка отклонена" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting VIP application {ApplicationId}", id);
            return StatusCode(500, new { message = "Ошибка при отклонении заявки", error = ex.Message });
        }
    }

    private async Task<Models.VipApplication?> GetVipApplicationSafely(int id)
    {
        try
        {
            return await _context.VipApplications
                .Include(a => a.User)
                .Include(a => a.Server)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (MySqlConnector.MySqlException mex) when (mex.Message?.Contains("Unknown column 'v.duration_days'") == true || mex.Message?.Contains("Unknown column 'vip_applications.duration_days'") == true)
        {
            var proj = await _context.VipApplications
                .Include(a => a.User)
                .Include(a => a.Server)
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.UserId,
                    a.Username,
                    a.SteamId,
                    a.ServerId,
                    serverName = a.Server != null ? a.Server.Name : "Неизвестный сервер",
                    a.HoursPerWeek,
                    a.Reason,
                    a.Status,
                    a.AdminId,
                    a.AdminComment,
                    VipGroup = a.VipGroup,
                    DurationDays = (int?)null,
                    a.CreatedAt,
                    a.ProcessedAt
                })
                .FirstOrDefaultAsync();

            if (proj == null) return null;

            return new Models.VipApplication
            {
                Id = proj.Id,
                UserId = proj.UserId,
                Username = proj.Username,
                SteamId = proj.SteamId,
                ServerId = proj.ServerId,
                Server = new Models.Server { Id = proj.ServerId, Name = proj.serverName },
                HoursPerWeek = proj.HoursPerWeek,
                Reason = proj.Reason,
                Status = proj.Status,
                AdminId = proj.AdminId,
                AdminComment = proj.AdminComment,
                VipGroup = proj.VipGroup,
                DurationDays = null,
                CreatedAt = proj.CreatedAt,
                ProcessedAt = proj.ProcessedAt
            };
        }
    }

private async Task<bool> TableHasColumnAsync(string tableName, string columnName)
{
    try
    {
        var conn = _context.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
        {
            await conn.OpenAsync();
        }

        await using var cmd = conn.CreateCommand();
        
        // Проверяем тип базы данных
        var isPostgres = conn.GetType().Name.Contains("Npgsql") || 
                         _context.Database.ProviderName?.Contains("Npgsql") == true;

        if (isPostgres)
        {
            cmd.CommandText = @"
                SELECT COUNT(*) 
                FROM information_schema.columns 
                WHERE table_name = @table AND column_name = @column";
        }
        else
        {
            cmd.CommandText = @"
                SELECT COUNT(*) 
                FROM information_schema.COLUMNS 
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
        }

        var pTable = cmd.CreateParameter();
        pTable.ParameterName = "@table";
        pTable.Value = tableName;
        cmd.Parameters.Add(pTable);

        var pCol = cmd.CreateParameter();
        pCol.ParameterName = "@column";
        pCol.Value = columnName;
        cmd.Parameters.Add(pCol);

        var res = await cmd.ExecuteScalarAsync();
        var cnt = Convert.ToInt64(res ?? 0);
        return cnt > 0;
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Error checking if column {Column} exists in table {Table}", columnName, tableName);
        return true; // Предполагаем что колонка есть
    }
}

    #endregion

    [HttpDelete("admin-privileges/{privilegeId}")]
    public async Task<IActionResult> RemoveAdminPrivilege(int privilegeId)
    {
        try
        {
            var sourceBans = HttpContext.RequestServices.GetRequiredService<ISourceBansService>();

            var privilege = await _context.UserAdminPrivileges
                .FirstOrDefaultAsync(p => p.Id == privilegeId);

            if (privilege == null)
            {
                return NotFound(new { message = "Admin привилегия не найдена" });
            }

            if (privilege.SourceBansAdminId.HasValue)
            {
                try
                {
                    var removed = await sourceBans.RemoveAdminAsync(privilege.SourceBansAdminId.Value);
                    if (!removed)
                    {
                        privilege.IsActive = false;
                        privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                        await _context.SaveChangesAsync();
                        return Ok(new { message = "Не удалось удалить внешний админ-аккаунт — привилегия помечена как неактивная" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing admin from SourceBans for privilege {PrivilegeId}", privilegeId);
                    privilege.IsActive = false;
                    privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();
                    await _context.SaveChangesAsync();
                    return StatusCode(500, new { message = "Ошибка при удалении внешнего админ-аккаунта" });
                }
            }

            _context.UserAdminPrivileges.Remove(privilege);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Admin привилегия успешно удалена" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing admin privilege {PrivilegeId}", privilegeId);
            return StatusCode(500, new { message = "Ошибка при удалении админ привилегии", error = ex.Message });
        }
    }

    #endregion

    #region VIP Groups

    [HttpGet("vip-groups")]
    public async Task<ActionResult<List<string>>> GetVipGroups()
    {
        try
        {
            var vipService = HttpContext.RequestServices.GetRequiredService<IVipService>();
            var groups = await GetVipGroupsFromDatabaseAsync(vipService);
            
            return Ok(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting VIP groups");
            return StatusCode(500, new { message = "Ошибка при получении списка VIP групп" });
        }
    }

    private async Task<List<string>> GetVipGroupsFromDatabaseAsync(IVipService vipService)
    {
        var groups = new List<string>();
        
        try
        {        
            var connectionString = await GetVipConnectionStringAsync();
            if (connectionString == null)
            {
                return new List<string> { "vip", "premium", "gold", "silver", "bronze" };
            }

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT `group` FROM vip_users ORDER BY `group`";
            
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var groupName = reader.GetString(0);
                if (!string.IsNullOrWhiteSpace(groupName))
                {
                    groups.Add(groupName);
                }
            }

            if (groups.Count == 0)
            {
                groups.AddRange(new[] { "vip", "premium", "gold", "silver", "bronze" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not get VIP groups from database, using defaults");
            groups.AddRange(new[] { "vip", "premium", "gold", "silver", "bronze" });
        }

        return groups.Distinct().OrderBy(g => g).ToList();
    }

    private async Task<string?> GetVipConnectionStringAsync()
    {
        var settings = await _context.VipSettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            return null;
        }

        return $"Server={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.Username};Password={settings.Password};";
    }

    #endregion
}
