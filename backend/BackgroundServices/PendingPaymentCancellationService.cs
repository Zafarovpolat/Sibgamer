using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Utils;

namespace backend.BackgroundServices;

public class PendingPaymentCancellationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PendingPaymentCancellationService> _logger;
    private const int STARTUP_DELAY_SECONDS = 75;
    private const int CHECK_INTERVAL_MINUTES = 2;
    private const int ERROR_RETRY_DELAY_SECONDS = 60;

    public PendingPaymentCancellationService(
        IServiceProvider serviceProvider,
        ILogger<PendingPaymentCancellationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PendingPaymentCancellationService: ожидание {Delay} секунд перед стартом...", STARTUP_DELAY_SECONDS);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(STARTUP_DELAY_SECONDS), stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        _logger.LogInformation("PendingPaymentCancellationService: запуск основного цикла");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CancelExpiredPendingPayments(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("PendingPaymentCancellationService: остановка по запросу");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отмене истёкших платежей");
                
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

        _logger.LogInformation("PendingPaymentCancellationService: остановлен");
    }

    private async Task CancelExpiredPendingPayments(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var now = DateTimeHelper.GetServerLocalTime();

        var expiredTransactions = await context.DonationTransactions
            .Where(t => 
                t.Status == "pending" && 
                t.PendingExpiresAt != null && 
                t.PendingExpiresAt <= now)
            .ToListAsync(stoppingToken);

        if (expiredTransactions.Any())
        {
            _logger.LogInformation("Найдено {Count} истёкших платежей для отмены", expiredTransactions.Count);

            foreach (var transaction in expiredTransactions)
            {
                transaction.Status = "cancelled";
                transaction.CancelledAt = now;
                
                _logger.LogInformation(
                    "Платёж {TransactionId} отменён. Тип: {Type}, Сумма: {Amount}",
                    transaction.TransactionId, transaction.Type, transaction.Amount);
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}