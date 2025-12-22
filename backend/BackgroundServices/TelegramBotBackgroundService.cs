using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using backend.Services;

namespace backend.BackgroundServices;

public class TelegramBotBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelegramBotBackgroundService> _logger;
    private TelegramBotClient? _botClient;
    private const int STARTUP_DELAY_SECONDS = 30;
    private const int CHECK_INTERVAL_MINUTES = 5;
    private const int ERROR_RETRY_DELAY_SECONDS = 60;

    public TelegramBotBackgroundService(IServiceProvider serviceProvider, ILogger<TelegramBotBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TelegramBotBackgroundService: ожидание {Delay} секунд перед стартом...", STARTUP_DELAY_SECONDS);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(STARTUP_DELAY_SECONDS), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("TelegramBotBackgroundService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await InitializeBotAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("TelegramBotBackgroundService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TelegramBotBackgroundService");
                
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(ERROR_RETRY_DELAY_SECONDS), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _logger.LogInformation("TelegramBotBackgroundService: остановлен");
    }

    private async Task InitializeBotAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();
        var token = await telegramService.GetBotTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogDebug("Telegram bot token not configured");
            return;
        }

        if (_botClient == null || _botClient.BotId.ToString() != token.Split(':')[0])
        {
            _botClient = new TelegramBotClient(token);
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions(),
                stoppingToken
            );
            _logger.LogInformation("Telegram bot started");
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text == null)
            return;

        var message = update.Message;
        var chatId = message.Chat.Id;
        var text = message.Text.ToLower();

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

            if (text == "/start")
            {
                await telegramService.SubscribeUserAsync(
                    chatId,
                    message.From?.Username,
                    message.From?.FirstName,
                    message.From?.LastName
                );

                await botClient.SendMessage(
                    new ChatId(chatId), 
                    "Привет! 👋\n\nВы успешно подписались на уведомления SIBGamer!", 
                    cancellationToken: cancellationToken
                );
            }
            else if (text == "/stop")
            {
                await telegramService.UnsubscribeUserAsync(chatId);
                await botClient.SendMessage(
                    new ChatId(chatId), 
                    "Вы отписались от уведомлений. Напишите /start чтобы подписаться снова.", 
                    cancellationToken: cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling Telegram message from {ChatId}", chatId);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogWarning(exception, "Telegram bot polling error");
        return Task.CompletedTask;
    }
}