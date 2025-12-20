using backend.Data;
using backend.Models;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace backend.Services;

public interface ITelegramService
{
    Task SetBotTokenAsync(string token);
    Task<string?> GetBotTokenAsync();
    Task SendNotificationToAllAsync(string title, string message);
    Task SubscribeUserAsync(long chatId, string? username, string? firstName, string? lastName);
    Task UnsubscribeUserAsync(long chatId);
    Task SendNewsNotificationAsync(News news);
    Task SendEventCreatedNotificationAsync(Event eventItem);
    Task SendEventStartedNotificationAsync(Event eventItem);
    Task SendEventEndedNotificationAsync(Event eventItem);
}

public class TelegramService : ITelegramService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TelegramService> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpFactory;
    private TelegramBotClient? _botClient;

    public TelegramService(ApplicationDbContext context, ILogger<TelegramService> logger, IConfiguration config, IHttpClientFactory httpFactory)
    {
        _context = context;
        _logger = logger;
        _config = config;
        _httpFactory = httpFactory;
    }

    public async Task SetBotTokenAsync(string token)
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "telegram_bot_token");
        if (setting == null)
        {
            setting = new SiteSetting
            {
                Key = "telegram_bot_token",
                Value = token,
                Category = "telegram",
                Description = "Токен Telegram бота для уведомлений",
                DataType = "string"
            };
            _context.SiteSettings.Add(setting);
        }
        else
        {
            setting.Value = token;
            setting.UpdatedAt = DateTimeHelper.GetServerLocalTime();
        }

        await _context.SaveChangesAsync();
        _botClient = new TelegramBotClient(token);
        _logger.LogInformation("Telegram bot token updated");
    }

    public async Task<string?> GetBotTokenAsync()
    {
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "telegram_bot_token");
        return setting?.Value;
    }

    public async Task SendNotificationToAllAsync(string title, string message)
    {
        var token = await GetBotTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Telegram bot token not set");
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(':')[0])
        {
            _botClient = new TelegramBotClient(token);
        }

        var subscribers = await _context.TelegramSubscribers
            .Where(s => s.IsActive)
            .ToListAsync();

        var fullMessage = $"🔔 *Важные новости от администрации*\n\n*{title}*\n\n{message}";

        foreach (var subscriber in subscribers)
        {
            try
            {
                await _botClient.SendMessage(
                    new ChatId(subscriber.ChatId),
                    fullMessage,
                    parseMode: ParseMode.Markdown
                );
                await Task.Delay(200); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to chat {ChatId}", subscriber.ChatId);
            }
        }

        _logger.LogInformation("Sent notification to {Count} Telegram subscribers", subscribers.Count);
    }

    public async Task SubscribeUserAsync(long chatId, string? username, string? firstName, string? lastName)
    {
        var subscriber = await _context.TelegramSubscribers.FirstOrDefaultAsync(s => s.ChatId == chatId);
        if (subscriber == null)
        {
            subscriber = new TelegramSubscriber
            {
                ChatId = chatId,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                SubscribedAt = DateTimeHelper.GetServerLocalTime()
            };
            _context.TelegramSubscribers.Add(subscriber);
        }
        else
        {
            subscriber.Username = username;
            subscriber.FirstName = firstName;
            subscriber.LastName = lastName;
            subscriber.IsActive = true;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("User {ChatId} subscribed to Telegram notifications", chatId);
    }

    public async Task UnsubscribeUserAsync(long chatId)
    {
        var subscriber = await _context.TelegramSubscribers.FirstOrDefaultAsync(s => s.ChatId == chatId);
        if (subscriber != null)
        {
            subscriber.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {ChatId} unsubscribed from Telegram notifications", chatId);
        }
    }

    public async Task SendNewsNotificationAsync(News news)
    {
        var token = await GetBotTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Telegram bot token not set");
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(':')[0])
        {
            _botClient = new TelegramBotClient(token);
        }

        var subscribers = await _context.TelegramSubscribers
            .Where(s => s.IsActive)
            .ToListAsync();

        if (!subscribers.Any())
        {
            return; 
        }

        var imageBase = await GetImageBaseUrlAsync();
        var frontUrl = await GetFrontendUrlAsync();
        var newsUrl = $"{frontUrl}/news/{news.Slug}";
        var caption = $"🔔 *НОВАЯ НОВОСТЬ SIBGAMER*\n\n";
        caption += $"📌 *{news.Title}*\n\n";
        
        if (!string.IsNullOrEmpty(news.Summary))
        {
            caption += $"💬 {news.Summary}\n\n";
        }
        
        caption += $"🔥 Читайте все новости на нашем сайте!";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("📖 Читать новость полностью", newsUrl)
            }
        });

        foreach (var subscriber in subscribers)
        {
            try
            {
                if (!string.IsNullOrEmpty(news.CoverImage))
                {
                    var imageUrl = news.CoverImage.StartsWith("http") ? news.CoverImage : $"{imageBase}{news.CoverImage}";

                    if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var parsedUri) &&
                        (parsedUri.Scheme == Uri.UriSchemeHttp || parsedUri.Scheme == Uri.UriSchemeHttps))
                    {
                        var sent = false;
                        try
                        {
                            await _botClient.SendPhoto(
                                chatId: new ChatId(subscriber.ChatId),
                                photo: InputFile.FromUri(imageUrl),
                                caption: caption,
                                parseMode: ParseMode.Markdown,
                                replyMarkup: inlineKeyboard
                            );
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "SendPhoto (URL) failed for news {NewsId} to chat {ChatId} — will attempt re-upload", news.Id, subscriber.ChatId);
                        }

                        if (!sent)
                        {
                            var reuploadOk = await TryDownloadAndSendPhotoAsync(subscriber.ChatId, imageUrl, caption, inlineKeyboard);
                            if (!reuploadOk)
                            {
                                _logger.LogWarning("Falling back to SendMessage for news {NewsId} to chat {ChatId}", news.Id, subscriber.ChatId);
                                await _botClient.SendMessage(
                                    chatId: new ChatId(subscriber.ChatId),
                                    text: caption,
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: inlineKeyboard
                                );
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Skipping SendPhoto for news {NewsId} because cover image URL is invalid: {ImageUrl}", news.Id, imageUrl);
                        await _botClient.SendMessage(
                            chatId: new ChatId(subscriber.ChatId),
                            text: caption,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: inlineKeyboard
                        );
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: new ChatId(subscriber.ChatId),
                        text: caption,
                        parseMode: ParseMode.Markdown,
                        replyMarkup: inlineKeyboard
                    );
                }

                await Task.Delay(200); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send news notification to chat {ChatId}", subscriber.ChatId);
            }
        }

        _logger.LogInformation("Sent news notification to {Count} Telegram subscribers", subscribers.Count);
    }

    public async Task SendEventCreatedNotificationAsync(Event eventItem)
    {
        var token = await GetBotTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Telegram bot token not set");
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(":")[0])
        {
            _botClient = new TelegramBotClient(token);
        }

        var subscribers = await _context.TelegramSubscribers
            .Where(s => s.IsActive)
            .ToListAsync();

        if (!subscribers.Any())
        {
            return; 
        }

        var imageBase = await GetImageBaseUrlAsync();
        var frontUrl = await GetFrontendUrlAsync();
        var eventUrl = $"{frontUrl}/events/{eventItem.Slug}";
        var caption = $"🎉 *НОВОЕ СОБЫТИЕ SIBGAMER*\n\n";
        caption += $"📌 *{eventItem.Title}*\n\n";
        
        if (!string.IsNullOrEmpty(eventItem.Summary))
        {
            caption += $"💬 {eventItem.Summary}\n\n";
        }
        
        caption += $"📅 Начало: {eventItem.StartDate:dd.MM.yyyy HH:mm}\n";
        caption += $"🏁 Окончание: {eventItem.EndDate:dd.MM.yyyy HH:mm}\n\n";
        caption += $"🔥 Узнайте все подробности на нашем сайте!";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("📖 Подробнее о событии", eventUrl)
            }
        });

        foreach (var subscriber in subscribers)
        {
            try
            {
                if (!string.IsNullOrEmpty(eventItem.CoverImage))
                {
                    var imageUrl = eventItem.CoverImage.StartsWith("http") ? eventItem.CoverImage : $"{imageBase}{eventItem.CoverImage}";
                    if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var parsedUri) &&
                        (parsedUri.Scheme == Uri.UriSchemeHttp || parsedUri.Scheme == Uri.UriSchemeHttps))
                    {
                        var sent = false;
                        try
                        {
                            await _botClient.SendPhoto(
                                chatId: new ChatId(subscriber.ChatId),
                                photo: InputFile.FromUri(imageUrl),
                                caption: caption,
                                parseMode: ParseMode.Markdown,
                                replyMarkup: inlineKeyboard
                            );
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "SendPhoto (URL) failed for event {EventId} to chat {ChatId} — will attempt re-upload", eventItem.Id, subscriber.ChatId);
                        }

                        if (!sent)
                        {
                            var ok = await TryDownloadAndSendPhotoAsync(subscriber.ChatId, imageUrl, caption, inlineKeyboard);
                            if (!ok)
                            {
                                _logger.LogWarning("Falling back to SendMessage for event {EventId} to chat {ChatId}", eventItem.Id, subscriber.ChatId);
                                await _botClient.SendMessage(
                                    chatId: new ChatId(subscriber.ChatId),
                                    text: caption,
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: inlineKeyboard
                                );
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Skipping SendPhoto for event {EventId} because cover image URL is invalid: {ImageUrl}", eventItem.Id, imageUrl);
                        await _botClient.SendMessage(
                            chatId: new ChatId(subscriber.ChatId),
                            text: caption,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: inlineKeyboard
                        );
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: new ChatId(subscriber.ChatId),
                        text: caption,
                        parseMode: ParseMode.Markdown,
                        replyMarkup: inlineKeyboard
                    );
                }

                await Task.Delay(200); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send event created notification to chat {ChatId}", subscriber.ChatId);
            }
        }

        _logger.LogInformation("Sent event created notification to {Count} Telegram subscribers", subscribers.Count);
    }

    public async Task SendEventStartedNotificationAsync(Event eventItem)
    {
        var token = await GetBotTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Telegram bot token not set");
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(":")[0])
        {
            _botClient = new TelegramBotClient(token);
        }

        var subscribers = await _context.TelegramSubscribers
            .Where(s => s.IsActive)
            .ToListAsync();

        if (!subscribers.Any())
        {
            return; 
        }

        var imageBase = await GetImageBaseUrlAsync();
        var frontUrl = await GetFrontendUrlAsync();
        var eventUrl = $"{frontUrl}/events/{eventItem.Slug}";
        var caption = $"🚀 *СОБЫТИЕ НАЧАЛОСЬ!*\n\n";
        caption += $"📌 *{eventItem.Title}*\n\n";
        
        if (!string.IsNullOrEmpty(eventItem.Summary))
        {
            caption += $"💬 {eventItem.Summary}\n\n";
        }
        
        caption += $"🏁 Окончание: {eventItem.EndDate:dd.MM.yyyy HH:mm}\n\n";
        caption += $"🔥 Присоединяйтесь к событию прямо сейчас!";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("🎮 Присоединиться", eventUrl)
            }
        });

        foreach (var subscriber in subscribers)
        {
            try
            {
                if (!string.IsNullOrEmpty(eventItem.CoverImage))
                {
                    var imageUrl = eventItem.CoverImage.StartsWith("http") ? eventItem.CoverImage : $"{imageBase}{eventItem.CoverImage}";
                    if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var parsedUri) &&
                        (parsedUri.Scheme == Uri.UriSchemeHttp || parsedUri.Scheme == Uri.UriSchemeHttps))
                    {
                        var sent = false;
                        try
                        {
                            await _botClient.SendPhoto(
                                chatId: new ChatId(subscriber.ChatId),
                                photo: InputFile.FromUri(imageUrl),
                                caption: caption,
                                parseMode: ParseMode.Markdown,
                                replyMarkup: inlineKeyboard
                            );
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "SendPhoto (URL) failed for event (started/ended) {EventId} to chat {ChatId} — will attempt re-upload", eventItem.Id, subscriber.ChatId);
                        }

                        if (!sent)
                        {
                            var ok = await TryDownloadAndSendPhotoAsync(subscriber.ChatId, imageUrl, caption, inlineKeyboard);
                            if (!ok)
                            {
                                _logger.LogWarning("Falling back to SendMessage for event (started/ended) {EventId} to chat {ChatId}", eventItem.Id, subscriber.ChatId);
                                await _botClient.SendMessage(
                                    chatId: new ChatId(subscriber.ChatId),
                                    text: caption,
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: inlineKeyboard
                                );
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Skipping SendPhoto for event (started/ended) {EventId} because cover image URL is invalid: {ImageUrl}", eventItem.Id, imageUrl);
                        await _botClient.SendMessage(
                            chatId: new ChatId(subscriber.ChatId),
                            text: caption,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: inlineKeyboard
                        );
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: new ChatId(subscriber.ChatId),
                        text: caption,
                        parseMode: ParseMode.Markdown,
                        replyMarkup: inlineKeyboard
                    );
                }

                await Task.Delay(200); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send event started notification to chat {ChatId}", subscriber.ChatId);
            }
        }

        _logger.LogInformation("Sent event started notification to {Count} Telegram subscribers", subscribers.Count);
    }

    public async Task SendEventEndedNotificationAsync(Event eventItem)
    {
        var token = await GetBotTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Telegram bot token not set");
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(":")[0])
        {
            _botClient = new TelegramBotClient(token);
        }

        var subscribers = await _context.TelegramSubscribers
            .Where(s => s.IsActive)
            .ToListAsync();

        if (!subscribers.Any())
        {
            return; 
        }

        var baseUrl = await GetImageBaseUrlAsync();
        var eventUrl = $"{baseUrl}/events/{eventItem.Slug}";
        var caption = $"🏁 *СОБЫТИЕ ЗАВЕРШИЛОСЬ*\n\n";
        caption += $"📌 *{eventItem.Title}*\n\n";
        
        if (!string.IsNullOrEmpty(eventItem.Summary))
        {
            caption += $"💬 {eventItem.Summary}\n\n";
        }
        
        caption += $"📊 Спасибо всем участникам!\n\n";
        caption += $"🔥 Посмотрите итоги и результаты на нашем сайте!";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("📊 Посмотреть итоги", eventUrl)
            }
        });

        foreach (var subscriber in subscribers)
        {
            try
            {
                if (!string.IsNullOrEmpty(eventItem.CoverImage))
                {
                    var imageUrl = eventItem.CoverImage.StartsWith("http") ? eventItem.CoverImage : $"{baseUrl}{eventItem.CoverImage}";
                    var sent = false;
                    if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var parsedUri) &&
                        (parsedUri.Scheme == Uri.UriSchemeHttp || parsedUri.Scheme == Uri.UriSchemeHttps))
                    {
                        try
                        {
                            await _botClient.SendPhoto(
                                chatId: new ChatId(subscriber.ChatId),
                                photo: InputFile.FromUri(imageUrl),
                                caption: caption,
                                parseMode: ParseMode.Markdown,
                                replyMarkup: inlineKeyboard
                            );
                            sent = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "SendPhoto (URL) failed for event ended {EventId} to chat {ChatId} — will attempt re-upload", eventItem.Id, subscriber.ChatId);
                        }

                        if (!sent)
                        {
                            var ok = await TryDownloadAndSendPhotoAsync(subscriber.ChatId, imageUrl, caption, inlineKeyboard);
                            if (!ok)
                            {
                                _logger.LogWarning("Falling back to SendMessage for event ended {EventId} to chat {ChatId}", eventItem.Id, subscriber.ChatId);
                                await _botClient.SendMessage(
                                    chatId: new ChatId(subscriber.ChatId),
                                    text: caption,
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: inlineKeyboard
                                );
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Skipping SendPhoto for event ended {EventId} because cover image URL is invalid: {ImageUrl}", eventItem.Id, imageUrl);
                        await _botClient.SendMessage(
                            chatId: new ChatId(subscriber.ChatId),
                            text: caption,
                            parseMode: ParseMode.Markdown,
                            replyMarkup: inlineKeyboard
                        );
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: new ChatId(subscriber.ChatId),
                        text: caption,
                        parseMode: ParseMode.Markdown,
                        replyMarkup: inlineKeyboard
                    );
                }

                await Task.Delay(200); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send event ended notification to chat {ChatId}", subscriber.ChatId);
            }
        }

        _logger.LogInformation("Sent event ended notification to {Count} Telegram subscribers", subscribers.Count);
    }

    private async Task<string> GetImageBaseUrlAsync()
    {
        var configured = _config?["ImageBaseUrl"];
        if (!string.IsNullOrWhiteSpace(configured))
            return configured.TrimEnd('/');
        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "telegram_image_base_url");
        if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            return setting.Value.TrimEnd('/');

        setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "site_api_base_url" || s.Key == "site_base_url");
        if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            return setting.Value.TrimEnd('/');

        return "https://api.sibgamer.com";
    }

    private async Task<bool> TryDownloadAndSendPhotoAsync(long chatId, string imageUrl, string caption, InlineKeyboardMarkup? replyMarkup)
    {
        if (_httpFactory == null)
        {
            _logger.LogWarning("HttpClientFactory is not available — cannot download image for reupload: {Url}", imageUrl);
            return false;
        }

        try
        {
            using var client = _httpFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(20);

            using var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Image download returned non-success status {Status} for {Url}", response.StatusCode, imageUrl);
                return false;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
            if (!contentType.StartsWith("image/"))
            {
                _logger.LogWarning("Image download for {Url} returned non-image content-type: {ContentType}", imageUrl, contentType);
                return false;
            }

            const long MaxBytes = 10 * 1024 * 1024; 
            var contentLength = response.Content.Headers.ContentLength;
            if (contentLength.HasValue && contentLength.Value > MaxBytes)
            {
                _logger.LogWarning("Image download for {Url} skipped: content-length {Length} exceeds max {Max}", imageUrl, contentLength.Value, MaxBytes);
                return false;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var ms = new MemoryStream();
            var buffer = new byte[81920];
            int read;
            long total = 0;
            while ((read = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                total += read;
                if (total > MaxBytes)
                {
                    ms.Dispose();
                    _logger.LogWarning("Image download for {Url} exceeded max size during streaming, aborting.", imageUrl);
                    return false;
                }
                ms.Write(buffer, 0, read);
            }

            ms.Position = 0;

            string fileName;
            try
            {
                var uri = new Uri(imageUrl);
                fileName = Path.GetFileName(uri.LocalPath);
                if (string.IsNullOrWhiteSpace(fileName)) fileName = "image.jpg";
            }
            catch
            {
                fileName = "image.jpg";
            }

            try
            {
                var input = InputFile.FromStream(ms, fileName);
                await _botClient!.SendPhoto(chatId: new ChatId(chatId), photo: input, caption: caption, parseMode: ParseMode.Markdown, replyMarkup: replyMarkup);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Re-upload SendPhoto failed for {Url} to chat {ChatId}", imageUrl, chatId);
                return false;
            }
            finally
            {
                try { ms.Dispose(); } catch { }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to download image for reupload: {Url}", imageUrl);
            return false;
        }
    }

    private async Task<string> GetFrontendUrlAsync()
    {
        var configured = _config?["FrontendUrl"];
        if (!string.IsNullOrWhiteSpace(configured))
            return configured.TrimEnd('/');

        var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "frontend_url" || s.Key == "site_frontend_url");
        if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            return setting.Value.TrimEnd('/');

        return "https://sibgamer.com";
    }
}
