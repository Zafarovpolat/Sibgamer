using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Utils;

namespace backend.BackgroundServices;

public class PendingPaymentCancellationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PendingPaymentCancellationService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

    public PendingPaymentCancellationService(
        IServiceProvider serviceProvider,
        ILogger<PendingPaymentCancellationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Запущен сервис отмены истёкших платежей");

        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CancelExpiredPendingPayments(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отмене истёкших платежей");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task CancelExpiredPendingPayments(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var now = DateTimeHelper.GetServerLocalTime();

        var expiredTransactions = await context.DonationTransactions
            .Where(t => 
                t.Status == "pending" && 
                t.PendingExpiresAt != null && 
                t.PendingExpiresAt <= now)
            .ToListAsync(cancellationToken);

        if (expiredTransactions.Any())
        {
            _logger.LogInformation($"Найдено {expiredTransactions.Count} истёкших платежей для отмены");

            foreach (var transaction in expiredTransactions)
            {
                transaction.Status = "cancelled";
                transaction.CancelledAt = now;
                
                _logger.LogInformation(
                    $"Платёж {transaction.TransactionId} отменён автоматически. " +
                    $"Тип: {transaction.Type}, Сумма: {transaction.Amount}, " +
                    $"Создан: {transaction.CreatedAt}, Истёк: {transaction.PendingExpiresAt}");
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
