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

    public TelegramBotBackgroundService(IServiceProvider serviceProvider, ILogger<TelegramBotBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();
                    var token = await telegramService.GetBotTokenAsync();

                    if (!string.IsNullOrEmpty(token))
                    {
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
                    else
                    {
                        _logger.LogWarning("Telegram bot token not set");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Telegram bot background service");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); 
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            var text = message.Text;

            using (var scope = _serviceProvider.CreateScope())
            {
                var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramService>();

                if (text.ToLower() == "/start")
                {
                    await telegramService.SubscribeUserAsync(
                        chatId,
                        message.From?.Username,
                        message.From?.FirstName,
                        message.From?.LastName
                    );

                    var welcomeMessage = "Привет! 👋\n\nВы успешно подписались на уведомления игрового сообщества SIBGamer!\n\nТеперь вы будете получать важные новости и обновления от администрации.";
                    await botClient.SendMessage(new ChatId(chatId), welcomeMessage, cancellationToken: cancellationToken);
                }
                else if (text.ToLower() == "/stop")
                {
                    await telegramService.UnsubscribeUserAsync(chatId);
                    var goodbyeMessage = "Вы отписались от уведомлений. Если передумаете, напишите /start";
                    await botClient.SendMessage(new ChatId(chatId), goodbyeMessage, cancellationToken: cancellationToken);
                }
            }
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram bot error");
        return Task.CompletedTask;
    }
}
