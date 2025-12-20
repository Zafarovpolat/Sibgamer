using System.Net;
using System.Net.Mail;
using backend.Data;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public interface IEmailService
{
    Task<bool> SendWelcomeEmailAsync(string toEmail, string username);
    Task<bool> SendPasswordResetEmailAsync(string toEmail, string username, string resetToken);
    Task<bool> SendAdminPurchaseConfirmationAsync(string toEmail, string username, string serverName, string adminPassword, DateTime expiresAt);
    Task<bool> SendAdminExtensionConfirmationAsync(string toEmail, string username, string serverName, DateTime newExpiresAt);
    Task<bool> SendVipPurchaseConfirmationAsync(string toEmail, string username, string serverName, string vipGroup, DateTime expiresAt);
    Task<(int successCount, int failureCount, List<string> errors)> SendBulkEmailAsync(string subject, string body);
    Task<(bool success, string message)> TestConnectionAsync(string testEmailAddress);
}

public class EmailService : IEmailService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmailService> _logger;
    private readonly string _frontendUrl;

    public EmailService(ApplicationDbContext context, ILogger<EmailService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _frontendUrl = configuration["FrontendUrl"] ?? "https://sibgamer.com";
    }

    private async Task<SmtpSettings?> GetSmtpSettingsAsync()
    {
        return await _context.SmtpSettings
            .Where(s => s.IsConfigured)
            .OrderByDescending(s => s.UpdatedAt)
            .FirstOrDefaultAsync();
    }

    private SmtpClient CreateSmtpClient(SmtpSettings settings)
    {
        var client = new SmtpClient(settings.Host, settings.Port)
        {
            Credentials = new NetworkCredential(settings.Username, settings.Password),
            EnableSsl = settings.EnableSsl,
            Timeout = 30000, 
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };
        
        return client;
    }

    public async Task<bool> SendWelcomeEmailAsync(string toEmail, string username)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                _logger.LogWarning("SMTP settings not configured. Cannot send welcome email.");
                return false;
            }

            var subject = $"Добро пожаловать на {settings.FromName}!";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2563eb;'>Привет, {username}! 👋</h2>
                        <p>Спасибо за регистрацию на нашем игровом портале!</p>
                        <p>Теперь у вас есть доступ к:</p>
                        <ul>
                            <li>Информации о серверах Counter-Strike Source</li>
                            <li>Новостям и событиям сообщества</li>
                            <li>Комментариям и обсуждениям</li>
                            <li>Личному профилю с интеграцией Steam</li>
                        </ul>
                        <p>Не забудьте добавить свой Steam ID в профиле!</p>
                        <p style='margin-top: 30px;'>
                            <a href='{_frontendUrl}' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Перейти на сайт
                            </a>
                        </p>
                        <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                            Если вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.
                        </p>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(settings, toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending welcome email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendAdminPurchaseConfirmationAsync(string toEmail, string username, string serverName, string adminPassword, DateTime expiresAt)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                _logger.LogWarning("SMTP settings not configured. Cannot send admin purchase confirmation email.");
                return false;
            }

            var subject = $"Админ-права активированы на сервере {serverName}";
            var expiryText = expiresAt == DateTime.MaxValue ? "навсегда" : $"до {expiresAt:dd.MM.yyyy HH:mm}";
            
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2563eb;'>Админ-права активированы! 🎉</h2>
                        <p>Привет, {username}!</p>
                        <p>Ваши админ-права на сервере <strong>{serverName}</strong> успешно активированы!</p>
                        
                        <div style='background-color: #f0f8ff; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #2563eb;'>
                            <h3 style='margin-top: 0; color: #2563eb;'>Инструкция по подключению:</h3>
                            <ol>
                                <li>Запустите Counter-Strike Source</li>
                                <li>Подключитесь к серверу <strong>{serverName}</strong></li>
                                <li>В консоли сервера введите: <code style='background-color: #e8f4f8; padding: 2px 6px; border-radius: 3px; font-family: monospace;'>setinfo _pw ""{adminPassword}""</code></li>
                                <li>Переподключитесь к серверу</li>
                            </ol>
                        </div>
                        
                        <p><strong>Пароль:</strong> <code style='background-color: #f5f5f5; padding: 4px 8px; border-radius: 4px; font-family: monospace; font-size: 16px;'>{adminPassword}</code></p>
                        <p><strong>Срок действия:</strong> {expiryText}</p>
                        
                        <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p style='margin: 0; color: #856404;'><strong>⚠️ Важно:</strong></p>
                            <ul style='color: #856404; margin: 10px 0 0 0;'>
                                <li>Пароль чувствителен к регистру</li>
                                <li>Вводите пароль без кавычек в команде setinfo</li>
                                <li>Пароль действует только на этом сервере</li>
                                <li>Вы можете изменить пароль в разделе ""Мои услуги"" в профиле</li>
                            </ul>
                        </div>
                        
                        <p style='margin-top: 30px;'>
                            <a href='{_frontendUrl}/profile' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Перейти в профиль
                            </a>
                        </p>
                        
                        <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                            Если у вас возникли проблемы с активацией, обратитесь к администрации сервера.
                        </p>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(settings, toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending admin purchase confirmation email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendAdminExtensionConfirmationAsync(string toEmail, string username, string serverName, DateTime newExpiresAt)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                _logger.LogWarning("SMTP settings not configured. Cannot send admin extension confirmation email.");
                return false;
            }

            var subject = $"Админ-права продлены на сервере {serverName}";
            var expiryText = newExpiresAt == DateTime.MaxValue ? "навсегда" : $"до {newExpiresAt:dd.MM.yyyy HH:mm}";
            
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2563eb;'>Админ-права успешно продлены! 🎉</h2>
                        <p>Привет, {username}!</p>
                        <p>Ваши админ-права на сервере <strong>{serverName}</strong> успешно продлены!</p>
                        
                        <div style='background-color: #f0f8ff; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #2563eb;'>
                            <h3 style='margin-top: 0; color: #2563eb;'>Информация о продлении:</h3>
                            <p><strong>Сервер:</strong> {serverName}</p>
                            <p><strong>Новый срок действия:</strong> {expiryText}</p>
                        </div>
                        
                        <div style='background-color: #e8f5e8; border: 1px solid #c8e6c9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p style='margin: 0; color: #2e7d32;'><strong>✅ Готово!</strong></p>
                            <p style='margin: 10px 0 0 0; color: #2e7d32;'>
                                Ваши админ-права автоматически продлены. Вы можете продолжать пользоваться ими без дополнительных действий.
                            </p>
                        </div>
                        
                        <p style='margin-top: 30px;'>
                            <a href='{_frontendUrl}/profile' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Перейти в профиль
                            </a>
                        </p>
                        
                        <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                            Если у вас возникли вопросы, обратитесь к администрации сервера.
                        </p>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(settings, toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending admin extension confirmation email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendVipPurchaseConfirmationAsync(string toEmail, string username, string serverName, string vipGroup, DateTime expiresAt)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                _logger.LogWarning("SMTP settings not configured. Cannot send VIP purchase confirmation email.");
                return false;
            }

            var subject = $"VIP статус активирован на сервере {serverName}";
            var expiryText = expiresAt == DateTime.MaxValue ? "навсегда" : $"до {expiresAt:dd.MM.yyyy HH:mm}";
            
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2563eb;'>VIP статус активирован! ⭐</h2>
                        <p>Привет, {username}!</p>
                        <p>Ваш VIP статус на сервере <strong>{serverName}</strong> успешно активирован!</p>
                        
                        <div style='background-color: #f0f8ff; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #2563eb;'>
                            <h3 style='margin-top: 0; color: #2563eb;'>Информация о VIP статусе:</h3>
                            <p><strong>Сервер:</strong> {serverName}</p>
                            <p><strong>VIP группа:</strong> {vipGroup}</p>
                            <p><strong>Срок действия:</strong> {expiryText}</p>
                        </div>
                        
                        <div style='background-color: #e8f5e8; border: 1px solid #c8e6c9; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p style='margin: 0; color: #2e7d32;'><strong>✅ Готово!</strong></p>
                            <p style='margin: 10px 0 0 0; color: #2e7d32;'>
                                Ваш VIP статус активирован. Теперь у вас есть доступ ко всем преимуществам VIP на сервере.
                            </p>
                        </div>
                        
                        <p style='margin-top: 30px;'>
                            <a href='{_frontendUrl}/profile' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Перейти в профиль
                            </a>
                        </p>
                        
                        <p style='margin-top: 30px; color: #666; font-size: 12px;'>
                            Если у вас возникли вопросы по VIP преимуществам, обратитесь к администрации сервера.
                        </p>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(settings, toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending VIP purchase confirmation email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string username, string resetToken)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                _logger.LogWarning("SMTP settings not configured. Cannot send password reset email.");
                return false;
            }

            var resetLink = $"{_frontendUrl}/reset-password?token={resetToken}";
            var subject = "Восстановление пароля";
            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2563eb;'>Восстановление пароля</h2>
                        <p>Привет, {username}!</p>
                        <p>Вы запросили восстановление пароля. Нажмите на кнопку ниже, чтобы создать новый пароль:</p>
                        <p style='margin: 30px 0;'>
                            <a href='{resetLink}' style='background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px;'>
                                Восстановить пароль
                            </a>
                        </p>
                        <p style='color: #666;'>Или скопируйте эту ссылку в браузер:</p>
                        <p style='background-color: #f5f5f5; padding: 10px; word-break: break-all; font-size: 12px;'>
                            {resetLink}
                        </p>
                        <p style='margin-top: 30px; color: #d32f2f; font-weight: bold;'>
                            ⚠️ Ссылка действительна в течение 1 часа.
                        </p>
                        <p style='color: #666; font-size: 12px;'>
                            Если вы не запрашивали восстановление пароля, просто проигнорируйте это письмо.
                        </p>
                    </div>
                </body>
                </html>
            ";

            return await SendEmailAsync(settings, toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<(int successCount, int failureCount, List<string> errors)> SendBulkEmailAsync(string subject, string body)
    {
        var successCount = 0;
        var failureCount = 0;
        var errors = new List<string>();

        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                errors.Add("SMTP settings not configured");
                return (0, 0, errors);
            }

            var users = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .ToListAsync();

            foreach (var user in users)
            {
                try
                {
                    var personalizedBody = body.Replace("{username}", user.Username);
                    var sent = await SendEmailAsync(settings, user.Email, subject, personalizedBody);
                    
                    if (sent)
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                        errors.Add($"Failed to send to {user.Email}");
                    }

                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    failureCount++;
                    errors.Add($"Error sending to {user.Email}: {ex.Message}");
                    _logger.LogError(ex, "Error sending bulk email to {Email}", user.Email);
                }
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Bulk email error: {ex.Message}");
            _logger.LogError(ex, "Error in bulk email sending");
        }

        return (successCount, failureCount, errors);
    }

    public async Task<(bool success, string message)> TestConnectionAsync(string testEmailAddress)
    {
        try
        {
            var settings = await GetSmtpSettingsAsync();
            if (settings == null)
            {
                return (false, "SMTP настройки не сконфигурированы");
            }

            _logger.LogInformation("Testing SMTP connection to {Host}:{Port} with SSL={SSL}, Username={User}", 
                settings.Host, settings.Port, settings.EnableSsl, settings.Username);

            using var client = CreateSmtpClient(settings);
            
            var testMessage = new MailMessage
            {
                From = new MailAddress(settings.FromEmail, settings.FromName),
                Subject = "Тестовое письмо - SMTP Configuration Test",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h3>✅ SMTP сервер работает корректно!</h3>
                        <p>Дата и время теста: {DateTimeHelper.GetServerLocalTime():dd.MM.yyyy HH:mm:ss}</p>
                        <p>Параметры подключения:</p>
                        <ul>
                            <li>Сервер: {settings.Host}</li>
                            <li>Порт: {settings.Port}</li>
                            <li>SSL: {(settings.EnableSsl ? "Включен" : "Выключен")}</li>
                            <li>Отправитель: {settings.FromName} ({settings.FromEmail})</li>
                        </ul>
                        <p style='color: #666; margin-top: 20px;'>
                            Это тестовое письмо подтверждает, что ваши настройки SMTP работают правильно.
                        </p>
                    </body>
                    </html>
                ",
                IsBodyHtml = true
            };
            testMessage.To.Add(testEmailAddress);

            _logger.LogInformation("Sending test email to {Email}", testEmailAddress);
            await client.SendMailAsync(testMessage);
            
            _logger.LogInformation("Test email sent successfully");
            return (true, $"✅ Тестовое письмо успешно отправлено на {testEmailAddress}!");
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "SMTP test connection failed");
            
            var errorMsg = "❌ Ошибка SMTP: ";
            if (ex.InnerException is System.Net.Sockets.SocketException)
            {
                errorMsg += "Не удалось подключиться к серверу. Проверьте хост, порт и настройки firewall.";
            }
            else if (ex.StatusCode == System.Net.Mail.SmtpStatusCode.MailboxUnavailable)
            {
                errorMsg += "Неверный email адрес отправителя или получателя.";
            }
            else if (ex.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase))
            {
                errorMsg += "Ошибка аутентификации. Проверьте имя пользователя и пароль.";
            }
            else
            {
                errorMsg += ex.Message;
            }
            
            return (false, errorMsg);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Test connection failed");
            return (false, $"❌ Ошибка: {ex.Message}");
        }
    }

    private async Task<bool> SendEmailAsync(SmtpSettings settings, string toEmail, string subject, string body)
    {
        try
        {
            using var client = CreateSmtpClient(settings);
            
            var message = new MailMessage
            {
                From = new MailAddress(settings.FromEmail, settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            return false;
        }
    }
}
