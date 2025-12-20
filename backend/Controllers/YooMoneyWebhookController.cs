using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Utils;
using System.Security.Cryptography;
using System.Text;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YooMoneyWebhookController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISourceBansService _sourceBansService;
    private readonly IVipService _vipService;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<YooMoneyWebhookController> _logger;
    private static readonly HashSet<string> _processedOperationIds = new HashSet<string>();

    public YooMoneyWebhookController(
        ApplicationDbContext context,
        ISourceBansService sourceBansService,
        IVipService vipService,
        IEmailService emailService,
        INotificationService notificationService,
        ILogger<YooMoneyWebhookController> logger)
    {
        _context = context;
        _sourceBansService = sourceBansService;
        _vipService = vipService;
        _emailService = emailService;
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpPost("notification")]
    public async Task<IActionResult> HandleNotification([FromForm] YooMoneyWebhookDto notification)
    {
        try
        {
            _logger.LogInformation("=== WEBHOOK RECEIVED ===");
            _logger.LogInformation("Raw notification data: Type={NotificationType}, Operation={OperationId}, Amount={Amount}, Currency={Currency}, Label={Label}, Sender={Sender}, Codepro={Codepro}, Sha1Hash={Sha1Hash}, DateTime={DateTime}",
                notification.notification_type, notification.operation_id, notification.amount, notification.currency, notification.label, notification.sender, notification.codepro, notification.sha1_hash, notification.datetime);

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogError("Model validation failed: {Errors}", errors);
                return BadRequest($"Invalid model: {errors}");
            }

            if (string.IsNullOrEmpty(notification.notification_type))
            {
                _logger.LogError("Missing required field: notification_type");
                return BadRequest("Missing notification_type");
            }
            if (string.IsNullOrEmpty(notification.operation_id))
            {
                _logger.LogError("Missing required field: operation_id");
                return BadRequest("Missing operation_id");
            }
            if (string.IsNullOrEmpty(notification.amount))
            {
                _logger.LogError("Missing required field: amount");
                return BadRequest("Missing amount");
            }
            if (string.IsNullOrEmpty(notification.label))
            {
                _logger.LogError("Missing required field: label");
                return BadRequest("Missing label");
            }
            if (string.IsNullOrEmpty(notification.sha1_hash))
            {
                _logger.LogError("Missing required field: sha1_hash");
                return BadRequest("Missing sha1_hash");
            }

            var settings = await _context.YooMoneySettings
                .Where(s => s.IsConfigured)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                _logger.LogWarning("YooMoney settings not configured");
                return BadRequest("Settings not configured");
            }

            if (!VerifySignature(notification, settings.SecretKey))
            {
                _logger.LogWarning("Invalid signature for notification {OperationId}", notification.operation_id);
                return BadRequest("Invalid signature");
            }

            if (notification.notification_type != "p2p-incoming" && notification.notification_type != "card-incoming")
            {
                _logger.LogInformation("Skipping notification type: {Type}. Only p2p-incoming and card-incoming are supported.", notification.notification_type);
                return Ok();
            }

            _logger.LogInformation("Processing {Type} payment. Operation={OperationId}, Amount={Amount}, Label={Label}", 
                notification.notification_type, notification.operation_id, notification.amount, notification.label);

            if (notification.codepro == "true")
            {
                _logger.LogWarning("Payment {OperationId} is code protected, skipping", notification.operation_id);
                return Ok();
            }

            if (notification.unaccepted == "true")
            {
                _logger.LogWarning("Payment {OperationId} is on hold (unaccepted), skipping", notification.operation_id);
                return Ok();
            }

            lock (_processedOperationIds)
            {
                if (_processedOperationIds.Contains(notification.operation_id))
                {
                    _logger.LogInformation("Notification {OperationId} already processed, ignoring duplicate", notification.operation_id);
                    return Ok();
                }
            }

            var transaction = await _context.DonationTransactions
                .Include(t => t.User)
                .Include(t => t.Tariff)
                .Include(t => t.TariffOption)
                .Include(t => t.VipTariff)
                .Include(t => t.VipTariffOption)
                .Include(t => t.Server)
                .Include(t => t.Privilege)
                .FirstOrDefaultAsync(t => t.Label == notification.label && t.Status == "pending");

            if (transaction == null)
            {
                var completedTransaction = await _context.DonationTransactions
                    .FirstOrDefaultAsync(t => t.Label == notification.label && t.Status == "completed");

                if (completedTransaction != null)
                {
                    _logger.LogInformation("Transaction {Label} already processed, ignoring duplicate notification", notification.label);
                    return Ok();
                }

                _logger.LogWarning("Transaction not found for label: {Label}", notification.label);
                return Ok(); 
            }

            if (!ValidateTransactionData(transaction, notification))
            {
                _logger.LogWarning("Transaction data validation failed for label: {Label}", notification.label);
                transaction.Status = "failed";
                await _context.SaveChangesAsync();
                return Ok();
            }

            var amountToCheck = notification.withdraw_amount ?? notification.amount;

            if (string.IsNullOrEmpty(amountToCheck) || !decimal.TryParse(amountToCheck, out var receivedAmount))
            {
                _logger.LogWarning("Invalid amount in notification for transaction {TransactionId}. Amount={Amount}, WithdrawAmount={WithdrawAmount}",
                    transaction.TransactionId, notification.amount, notification.withdraw_amount);
                transaction.Status = "failed";
                await _context.SaveChangesAsync();
                return Ok();
            }

            if (receivedAmount != transaction.Amount)
            {
                _logger.LogWarning("Amount mismatch for transaction {TransactionId}. Expected: {Expected}, Got: {Actual} (type: {Type}, amount: {Amount}, withdraw_amount: {WithdrawAmount})",
                    transaction.TransactionId, transaction.Amount, receivedAmount, notification.notification_type, notification.amount, notification.withdraw_amount);
                
                transaction.Status = "failed";
                await _context.SaveChangesAsync();
                return Ok();
            }

            _logger.LogInformation("Amount verified for {Type}: Expected={Expected}, Received={Received} (amount={Amount}, withdraw_amount={WithdrawAmount})",
                notification.notification_type, transaction.Amount, receivedAmount, notification.amount, notification.withdraw_amount);

            transaction.Status = "completed";
            transaction.CompletedAt = DateTimeHelper.GetServerLocalTime();
            transaction.PaymentMethod = "yoomoney";

            lock (_processedOperationIds)
            {
                _processedOperationIds.Add(notification.operation_id);
            }

            if (transaction.Type == "donation")
            {
                _logger.LogInformation("Processing donation for transaction {TransactionId}: User={UserId}, Amount={Amount}",
                    transaction.TransactionId, transaction.UserId, transaction.Amount);
            }
            else if (transaction.Type == "admin_purchase" && transaction.TariffId.HasValue && transaction.ServerId.HasValue)
            {
                await ProcessAdminPurchase(transaction);
            }
            else if (transaction.Type == "vip_purchase" && transaction.VipTariffId.HasValue && transaction.ServerId.HasValue)
            {
                await ProcessVipPurchase(transaction);
            }
            else if (transaction.Type == "extend" && transaction.PrivilegeId.HasValue)
            {
                await ProcessAdminExtension(transaction);
            }

            await _context.SaveChangesAsync();
            await CreateUserNotification(transaction);
            await CreateAdminNotification(transaction);

            _logger.LogInformation("✅ Successfully processed {NotificationType} payment: Type={TransactionType}, TransactionId={TransactionId}, User={UserId}, Amount={Amount} RUB, Server={ServerName}, Operation={OperationId}",
                notification.notification_type, transaction.Type, transaction.TransactionId, transaction.UserId, transaction.Amount, transaction.Server?.Name ?? "N/A", notification.operation_id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error processing YooMoney notification");
            return StatusCode(500, "Internal error");
        }
    }

    private bool VerifySignature(YooMoneyWebhookDto notification, string secretKey)
    {
        try
        {
            var signatureString = $"{notification.notification_type}&" +
                                 $"{notification.operation_id}&" +
                                 $"{notification.amount}&" +
                                 $"{notification.currency}&" +
                                 $"{notification.datetime}&" +
                                 $"{notification.sender ?? ""}&" + 
                                 $"{notification.codepro}&" +
                                 $"{secretKey}&" +
                                 $"{notification.label ?? ""}";    

            using var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(signatureString));
            var computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            var receivedHash = notification.sha1_hash?.ToLowerInvariant() ?? "";
            var isValid = computedHash == receivedHash;

            if (!isValid)
            {
                _logger.LogWarning("Signature verification failed!");
                _logger.LogWarning("Signature string: {SignatureString}", signatureString);
                _logger.LogWarning("Computed hash: {ComputedHash}", computedHash);
                _logger.LogWarning("Received hash: {ReceivedHash}", receivedHash);
            }
            else
            {
                _logger.LogInformation("Signature verified successfully for {Type} payment", notification.notification_type);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying signature");
            return false;
        }
    }

    private bool ValidateTransactionData(DonationTransaction transaction, YooMoneyWebhookDto notification)
    {
        try
        {
            if (string.IsNullOrEmpty(transaction.Label) || transaction.Label != notification.label)
            {
                _logger.LogWarning("Label mismatch: expected {Expected}, got {Actual}",
                    transaction.Label, notification.label);
                return false;
            }

            if (transaction.Type == "donation")
            {
                var expectedLabel = $"donation_{transaction.UserId}_{transaction.TransactionId}";
                if (transaction.Label != expectedLabel)
                {
                    _logger.LogWarning("Invalid donation label format: {Label}", transaction.Label);
                    return false;
                }
            }

            else if (transaction.Type == "admin_purchase")
            {
                var expectedLabel = $"admin_{transaction.UserId}_{transaction.ServerId}_{transaction.TariffOptionId}_{transaction.TransactionId}";
                if (transaction.Label != expectedLabel)
                {
                    _logger.LogWarning("Invalid admin purchase label format: {Label}", transaction.Label);
                    return false;
                }
            }

            else if (transaction.Type == "vip_purchase")
            {
                var expectedLabel = $"vip_{transaction.UserId}_{transaction.ServerId}_{transaction.VipTariffOptionId}_{transaction.TransactionId}";
                if (transaction.Label != expectedLabel)
                {
                    _logger.LogWarning("Invalid VIP purchase label format: {Label}", transaction.Label);
                    return false;
                }
            }

            else if (transaction.Type == "extend")
            {
                if (transaction.Label != transaction.TransactionId)
                {
                    _logger.LogWarning("Invalid extend label format: {Label}", transaction.Label);
                    return false;
                }
            }

            if (transaction.UserId.HasValue && transaction.User != null)
            {
                if (transaction.Type == "donation" || transaction.Type == "admin_purchase")
                {
                    if (transaction.UserId != transaction.User.Id)
                    {
                        _logger.LogWarning("User mismatch for transaction {TransactionId}", transaction.TransactionId);
                        return false;
                    }
                }
            }

            _logger.LogDebug("Transaction data validation passed for {TransactionId}", transaction.TransactionId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating transaction data");
            return false;
        }
    }

    private async Task ProcessAdminPurchase(DonationTransaction transaction)
    {
        try
        {
            if (transaction.User == null || string.IsNullOrEmpty(transaction.SteamId) || 
                transaction.TariffOption == null || transaction.Tariff == null || transaction.Server == null)
            {
                _logger.LogError("Invalid transaction data for admin purchase");
                return;
            }

            _logger.LogInformation("Processing admin purchase: Transaction={TransactionId}, User={UserId}, SteamId={SteamId}, Server={ServerId}, Tariff={TariffId}, Password={HasPassword}",
                transaction.TransactionId, transaction.UserId, transaction.SteamId, transaction.ServerId, transaction.TariffId, !string.IsNullOrEmpty(transaction.AdminPassword));

            var tariffOption = transaction.TariffOption;
            var tariff = transaction.Tariff;
            var durationSeconds = tariffOption.DurationDays == 0 ? 0 : tariffOption.DurationDays * 24 * 60 * 60;
            var adminId = await _sourceBansService.AddAdminAsync(
                transaction.SteamId,
                transaction.User.Username,
                tariff.Flags,
                tariff.GroupName,
                tariff.Immunity,
                durationSeconds,
                transaction.AdminPassword
            );

            if (adminId == null)
            {
                _logger.LogError("Failed to add admin to SourceBans for transaction {TransactionId}", transaction.TransactionId);
                return;
            }

            var existingPrivilege = await _context.UserAdminPrivileges
                .FirstOrDefaultAsync(p => 
                    p.UserId == transaction.UserId && 
                    p.ServerId == transaction.ServerId && 
                    p.IsActive);

            DateTime expiresAt;
            if (existingPrivilege != null)
            {
                var newExpiresAt = tariffOption.DurationDays == 0 
                    ? DateTime.MaxValue 
                    : (existingPrivilege.ExpiresAt > DateTimeHelper.GetServerLocalTime() 
                        ? existingPrivilege.ExpiresAt.AddDays(tariffOption.DurationDays)
                        : DateTimeHelper.GetServerLocalTime().AddDays(tariffOption.DurationDays));
                
                existingPrivilege.ExpiresAt = newExpiresAt;
                existingPrivilege.TariffOptionId = tariffOption.Id;
                existingPrivilege.AdminPassword = transaction.AdminPassword;
                expiresAt = newExpiresAt;
                
                _logger.LogInformation("Extended admin privilege for user {UserId} on server {ServerId} until {ExpiresAt}",
                    transaction.UserId, transaction.ServerId, existingPrivilege.ExpiresAt);
            }
            else
            {
                var privilege = new UserAdminPrivilege
                {
                    UserId = transaction.UserId!.Value,
                    SteamId = transaction.SteamId,
                    ServerId = transaction.ServerId!.Value,
                    TariffId = transaction.TariffId!.Value,
                    TariffOptionId = tariffOption.Id,
                    TransactionId = transaction.Id,
                    Flags = tariff.Flags,
                    GroupName = tariff.GroupName,
                    Immunity = tariff.Immunity,
                    StartsAt = DateTimeHelper.GetServerLocalTime(),
                    ExpiresAt = tariffOption.DurationDays == 0 ? DateTime.MaxValue : DateTimeHelper.GetServerLocalTime().AddDays(tariffOption.DurationDays),
                    IsActive = true,
                    SourceBansAdminId = adminId,
                    AdminPassword = transaction.AdminPassword
                };

                _context.UserAdminPrivileges.Add(privilege);
                expiresAt = privilege.ExpiresAt;
                
                _logger.LogInformation("Created new admin privilege for user {UserId} on server {ServerId}",
                    transaction.UserId, transaction.ServerId);
            }

            if (!string.IsNullOrEmpty(transaction.User.Email) && !string.IsNullOrEmpty(transaction.AdminPassword))
            {
                await _emailService.SendAdminPurchaseConfirmationAsync(
                    transaction.User.Email,
                    transaction.User.Username,
                    transaction.Server.Name,
                    transaction.AdminPassword,
                    expiresAt
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing admin purchase for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    private async Task ProcessAdminExtension(DonationTransaction transaction)
    {
        try
        {
            if (transaction.PrivilegeId == null || transaction.TariffOption == null || transaction.Privilege == null)
            {
                _logger.LogError("Invalid transaction data for admin extension");
                return;
            }

            _logger.LogInformation("Processing admin extension: Transaction={TransactionId}, Privilege={PrivilegeId}, TariffOption={TariffOptionId}, Duration={DurationDays} days",
                transaction.TransactionId, transaction.PrivilegeId, transaction.TariffOptionId, transaction.TariffOption.DurationDays);

            var privilege = transaction.Privilege;
            var tariffOption = transaction.TariffOption;
            var durationSeconds = tariffOption.DurationDays == 0 ? 0 : tariffOption.DurationDays * 24 * 60 * 60;

            if (privilege.SourceBansAdminId.HasValue)
            {
                var success = await _sourceBansService.UpdateAdminExpiryAsync(privilege.SourceBansAdminId.Value, durationSeconds);
                if (!success)
                {
                    _logger.LogError("Failed to extend admin in SourceBans for transaction {TransactionId}", transaction.TransactionId);
                    return;
                }
            }

            var newExpiresAt = tariffOption.DurationDays == 0 
                ? DateTime.MaxValue 
                : (privilege.ExpiresAt > DateTimeHelper.GetServerLocalTime() 
                    ? privilege.ExpiresAt.AddDays(tariffOption.DurationDays)
                    : DateTimeHelper.GetServerLocalTime().AddDays(tariffOption.DurationDays));
            
            privilege.ExpiresAt = newExpiresAt;
            privilege.TariffOptionId = tariffOption.Id;
            privilege.UpdatedAt = DateTimeHelper.GetServerLocalTime();

            if (!string.IsNullOrEmpty(transaction.AdminPassword))
            {
                privilege.AdminPassword = transaction.AdminPassword;
            }

            _logger.LogInformation("Extended admin privilege {PrivilegeId} until {ExpiresAt}",
                privilege.Id, privilege.ExpiresAt);

            if (!string.IsNullOrEmpty(transaction.User?.Email))
            {
                await _emailService.SendAdminExtensionConfirmationAsync(
                    transaction.User.Email,
                    transaction.User.Username,
                    privilege.Server?.Name ?? "Неизвестный сервер",
                    newExpiresAt
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing admin extension for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    private async Task ProcessVipPurchase(DonationTransaction transaction)
    {
        try
        {
            if (transaction.User == null || string.IsNullOrEmpty(transaction.SteamId) || 
                transaction.VipTariffOptionId == null || transaction.VipTariffId == null || transaction.Server == null)
            {
                _logger.LogError("Invalid transaction data for VIP purchase");
                return;
            }

            _logger.LogInformation("Processing VIP purchase: Transaction={TransactionId}, User={UserId}, SteamId={SteamId}, Server={ServerId}, Tariff={TariffId}",
                transaction.TransactionId, transaction.UserId, transaction.SteamId, transaction.ServerId, transaction.VipTariffId);

            var vipTariff = await _context.VipTariffs.FindAsync(transaction.VipTariffId.Value);
            var vipTariffOption = await _context.VipTariffOptions.FindAsync(transaction.VipTariffOptionId.Value);

            if (vipTariff == null || vipTariffOption == null)
            {
                _logger.LogError("VIP tariff or tariff option not found for transaction {TransactionId}", transaction.TransactionId);
                return;
            }

            var durationSeconds = vipTariffOption.DurationDays == 0 ? 0 : vipTariffOption.DurationDays * 24 * 60 * 60;
            var success = await _vipService.AddVipAsync(
                transaction.SteamId,
                transaction.User.Username,
                vipTariff.GroupName ?? "vip",
                durationSeconds
            );

            if (!success)
            {
                _logger.LogError("Failed to add VIP to VIP database for transaction {TransactionId}", transaction.TransactionId);
                return;
            }

            var existingPrivilege = await _context.UserVipPrivileges
                .FirstOrDefaultAsync(p => 
                    p.UserId == transaction.UserId && 
                    p.ServerId == transaction.ServerId && 
                    p.IsActive);

            DateTime expiresAt;
            if (existingPrivilege != null)
            {
                var newExpiresAt = vipTariffOption.DurationDays == 0 
                    ? DateTime.MaxValue 
                    : (existingPrivilege.ExpiresAt > DateTimeHelper.GetServerLocalTime() 
                        ? existingPrivilege.ExpiresAt.AddDays(vipTariffOption.DurationDays)
                        : DateTimeHelper.GetServerLocalTime().AddDays(vipTariffOption.DurationDays));
                existingPrivilege.ExpiresAt = newExpiresAt;
                existingPrivilege.GroupName = vipTariff.GroupName ?? string.Empty;
                expiresAt = newExpiresAt;
                
                _logger.LogInformation("Extended VIP privilege for user {UserId} on server {ServerId} until {ExpiresAt}",
                    transaction.UserId, transaction.ServerId, existingPrivilege.ExpiresAt);
            }
            else
            {
                var privilege = new UserVipPrivilege
                {
                    UserId = transaction.UserId!.Value,
                    SteamId = transaction.SteamId!,
                    ServerId = transaction.ServerId!.Value,
                    TariffId = vipTariff.Id,
                    TariffOptionId = vipTariffOption.Id,
                    TransactionId = transaction.Id,
                    GroupName = vipTariff.GroupName!,
                    StartsAt = DateTimeHelper.GetServerLocalTime(),
                    ExpiresAt = vipTariffOption.DurationDays == 0 ? DateTime.MaxValue : DateTimeHelper.GetServerLocalTime().AddDays(vipTariffOption.DurationDays),
                    IsActive = true
                };

                _context.UserVipPrivileges.Add(privilege);
                expiresAt = privilege.ExpiresAt;
                
                _logger.LogInformation("Created new VIP privilege for user {UserId} on server {ServerId}",
                    transaction.UserId, transaction.ServerId);
            }

            if (!string.IsNullOrEmpty(transaction.User?.Email) && !string.IsNullOrEmpty(vipTariff.GroupName))
            {
                await _emailService.SendVipPurchaseConfirmationAsync(
                    transaction.User.Email,
                    transaction.User.Username,
                    transaction.Server?.Name ?? "Неизвестный сервер",
                    vipTariff.GroupName,
                    expiresAt
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing VIP purchase for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        var scheme = Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? Request.Scheme;
        var host = Request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? Request.Host.ToString();
        
        return Ok(new { 
            message = "YooMoney webhook endpoint is working", 
            timestamp = DateTimeHelper.GetServerLocalTime(),
            endpoint = "POST /api/YooMoneyWebhook/notification",
            fullUrl = $"{scheme}://{host}/api/YooMoneyWebhook/notification",
            correctUrl = "https://api.sibgamer.com/api/YooMoneyWebhook/notification",
            expectedFields = new[] { 
                "notification_type", "operation_id", "amount", "currency", 
                "datetime", "sender", "codepro", "label", "sha1_hash" 
            },
            instructions = "Укажите correctUrl в настройках HTTP-уведомлений ЮMoney"
        });
    }

    [HttpPost("test-notification")]
    public async Task<IActionResult> TestNotification([FromForm] string label, [FromForm] string amount)
    {
        try
        {
            var transaction = await _context.DonationTransactions
                .Include(t => t.User)
                .Include(t => t.Tariff)
                .Include(t => t.TariffOption)
                .Include(t => t.VipTariff)
                .Include(t => t.VipTariffOption)
                .Include(t => t.Server)
                .FirstOrDefaultAsync(t => t.Label == label);

            if (transaction == null)
            {
                return NotFound(new { message = $"Transaction with label {label} not found" });
            }

            return Ok(new {
                message = "Transaction found",
                transaction = new {
                    transactionId = transaction.TransactionId,
                    label = transaction.Label,
                    amount = transaction.Amount,
                    status = transaction.Status,
                    type = transaction.Type,
                    userId = transaction.UserId,
                    createdAt = transaction.CreatedAt
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in test notification");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    private async Task CreateUserNotification(DonationTransaction transaction)
    {
        try
        {
            if (transaction.User == null || transaction.UserId == null)
                return;

            string title, message, type;

            if (transaction.Type == "donation")
            {
                title = "Спасибо за поддержку! 🎉";
                message = $"Ваш донат на сумму {transaction.Amount} ₽ успешно получен. Спасибо за поддержку проекта!";
                type = "donation";
            }
            else if (transaction.Type == "admin_purchase")
            {
                title = "Админ-права активированы! 🛡️";
                message = $"Ваши админ-права на сервере {transaction.Server?.Name} успешно активированы. " +
                         $"Пароль: {transaction.AdminPassword}. " +
                         $"Следуйте инструкциям из email для подключения.";
                type = "admin_purchase";
            }
            else if (transaction.Type == "vip_purchase")
            {
                title = "VIP статус активирован! ⭐";
                message = $"Ваш VIP статус на сервере {transaction.Server?.Name} успешно активирован. " +
                         $"Следуйте инструкциям из email для получения преимуществ.";
                type = "vip_purchase";
            }
            else
            {
                return;
            }

            await _notificationService.CreateNotificationAsync(
                transaction.UserId.Value,
                title,
                message,
                type,
                transaction.Id
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user notification for transaction {TransactionId}", transaction.TransactionId);
        }
    }

    private async Task CreateAdminNotification(DonationTransaction transaction)
    {
        try
        {
            string title, message, type;

            if (transaction.Type == "donation")
            {
                title = "Новый донат получен 💰";
                message = $"Пользователь {transaction.User?.Username} сделал донат на сумму {transaction.Amount} ₽";
                type = "admin_notification";
            }
            else if (transaction.Type == "admin_purchase")
            {
                title = "Новая покупка админ-прав 🛡️";
                message = $"Пользователь {transaction.User?.Username} купил админ-права на сервере {transaction.Server?.Name} " +
                         $"на сумму {transaction.Amount} ₽";
                type = "admin_notification";
            }
            else if (transaction.Type == "vip_purchase")
            {
                title = "Новая покупка VIP статуса ⭐";
                message = $"Пользователь {transaction.User?.Username} купил VIP статус на сервере {transaction.Server?.Name} " +
                         $"на сумму {transaction.Amount} ₽";
                type = "admin_notification";
            }
            else
            {
                return;
            }

            await _notificationService.CreateAdminNotificationAsync(
                title,
                message,
                type,
                transaction.Id
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating admin notification for transaction {TransactionId}", transaction.TransactionId);
        }
    }
}
