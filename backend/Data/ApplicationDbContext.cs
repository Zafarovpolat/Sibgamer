using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ============================================
    // DbSets
    // ============================================
    public DbSet<User> Users { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<SliderImage> SliderImages { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<NewsMedia> NewsMedia { get; set; }
    public DbSet<NewsComment> NewsComments { get; set; }
    public DbSet<NewsLike> NewsLikes { get; set; }
    public DbSet<SiteSetting> SiteSettings { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventMedia> EventMedia { get; set; }
    public DbSet<EventComment> EventComments { get; set; }
    public DbSet<EventLike> EventLikes { get; set; }
    public DbSet<NewsView> NewsViews { get; set; }
    public DbSet<EventView> EventViews { get; set; }
    public DbSet<CustomPageView> CustomPageViews { get; set; }
    public DbSet<SmtpSettings> SmtpSettings { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<YooMoneySettings> YooMoneySettings { get; set; }
    public DbSet<SourceBansSettings> SourceBansSettings { get; set; }
    public DbSet<VipSettings> VipSettings { get; set; }
    public DbSet<AdminTariffGroup> AdminTariffGroups { get; set; }
    public DbSet<AdminTariff> AdminTariffs { get; set; }
    public DbSet<AdminTariffOption> AdminTariffOptions { get; set; }
    public DbSet<VipTariff> VipTariffs { get; set; }
    public DbSet<VipTariffOption> VipTariffOptions { get; set; }
    public DbSet<DonationPackage> DonationPackages { get; set; }
    public DbSet<DonationTransaction> DonationTransactions { get; set; }
    public DbSet<UserAdminPrivilege> UserAdminPrivileges { get; set; }
    public DbSet<UserVipPrivilege> UserVipPrivileges { get; set; }
    public DbSet<VipApplication> VipApplications { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<BlockedIp> BlockedIps { get; set; }
    public DbSet<TelegramSubscriber> TelegramSubscribers { get; set; }
    public DbSet<CustomPage> CustomPages { get; set; }
    public DbSet<CustomPageMedia> CustomPageMedia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Определяем используется ли PostgreSQL
        var isPostgres = Database.ProviderName?.Contains("Npgsql") == true 
            || Database.ProviderName?.Contains("PostgreSQL") == true;

        if (isPostgres)
        {
            // PostgreSQL: применяем snake_case именование
            ApplySnakeCaseNaming(modelBuilder);
        }

        // ============================================
        // User
        // ============================================
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // ============================================
        // News
        // ============================================
        modelBuilder.Entity<News>()
            .HasIndex(n => n.Slug)
            .IsUnique();

        modelBuilder.Entity<News>()
            .HasOne(n => n.Author)
            .WithMany()
            .HasForeignKey(n => n.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NewsComment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NewsLike>()
            .HasIndex(l => new { l.NewsId, l.UserId })
            .IsUnique();

        // ============================================
        // Site Settings
        // ============================================
        modelBuilder.Entity<SiteSetting>()
            .HasIndex(s => s.Key)
            .IsUnique();

        modelBuilder.Entity<SiteSetting>()
            .HasIndex(s => s.Category);

        // ============================================
        // Events
        // ============================================
        modelBuilder.Entity<Event>()
            .HasIndex(e => e.Slug)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventComment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventLike>()
            .HasIndex(l => new { l.EventId, l.UserId })
            .IsUnique();

        // ============================================
        // Views
        // ============================================
        modelBuilder.Entity<NewsView>()
            .HasIndex(v => new { v.NewsId, v.UserId, v.ViewDate });

        modelBuilder.Entity<NewsView>()
            .HasIndex(v => new { v.NewsId, v.IpAddress, v.ViewDate });

        modelBuilder.Entity<EventView>()
            .HasIndex(v => new { v.EventId, v.UserId, v.ViewDate });

        modelBuilder.Entity<EventView>()
            .HasIndex(v => new { v.EventId, v.IpAddress, v.ViewDate });

        modelBuilder.Entity<CustomPageView>()
            .HasIndex(v => new { v.CustomPageId, v.UserId, v.ViewDate });

        modelBuilder.Entity<CustomPageView>()
            .HasIndex(v => new { v.CustomPageId, v.IpAddress, v.ViewDate });

        // ============================================
        // Password Reset
        // ============================================
        modelBuilder.Entity<PasswordResetToken>()
            .HasIndex(t => t.Token);

        modelBuilder.Entity<PasswordResetToken>()
            .HasIndex(t => new { t.UserId, t.IsUsed });

        // ============================================
        // Tariffs
        // ============================================
        modelBuilder.Entity<AdminTariffGroup>()
            .HasIndex(g => new { g.ServerId, g.IsActive });

        modelBuilder.Entity<AdminTariff>()
            .HasIndex(t => new { t.ServerId, t.IsActive });

        modelBuilder.Entity<AdminTariffOption>()
            .HasIndex(o => new { o.TariffId, o.IsActive });

        modelBuilder.Entity<VipTariff>()
            .HasIndex(t => new { t.ServerId, t.IsActive });

        modelBuilder.Entity<VipTariffOption>()
            .HasIndex(o => new { o.TariffId, o.IsActive });

        // ============================================
        // Donations
        // ============================================
        modelBuilder.Entity<DonationTransaction>()
            .HasIndex(t => t.TransactionId)
            .IsUnique();

        modelBuilder.Entity<DonationTransaction>()
            .HasIndex(t => new { t.UserId, t.Status });

        modelBuilder.Entity<DonationTransaction>()
            .HasIndex(t => t.SteamId);

        // ============================================
        // Privileges
        // ============================================
        modelBuilder.Entity<UserAdminPrivilege>()
            .HasIndex(p => new { p.UserId, p.ServerId, p.IsActive });

        modelBuilder.Entity<UserAdminPrivilege>()
            .HasIndex(p => new { p.SteamId, p.ServerId, p.IsActive });

        modelBuilder.Entity<UserAdminPrivilege>()
            .HasIndex(p => p.ExpiresAt);

        modelBuilder.Entity<UserVipPrivilege>()
            .HasIndex(p => new { p.UserId, p.ServerId, p.IsActive });

        modelBuilder.Entity<UserVipPrivilege>()
            .HasIndex(p => new { p.SteamId, p.ServerId, p.IsActive });

        modelBuilder.Entity<UserVipPrivilege>()
            .HasIndex(p => p.ExpiresAt);

        // ============================================
        // Notifications
        // ============================================
        modelBuilder.Entity<Notification>()
            .HasIndex(n => new { n.UserId, n.IsRead });

        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.CreatedAt);

        // ============================================
        // VIP Applications
        // ============================================
        modelBuilder.Entity<VipApplication>()
            .HasIndex(a => new { a.UserId, a.ServerId, a.Status });

        // ============================================
        // Blocked IPs
        // ============================================
        modelBuilder.Entity<BlockedIp>()
            .HasIndex(b => b.IpAddress)
            .IsUnique();

        // ============================================
        // Telegram
        // ============================================
        modelBuilder.Entity<TelegramSubscriber>()
            .HasIndex(t => t.ChatId)
            .IsUnique();

        modelBuilder.Entity<TelegramSubscriber>()
            .HasIndex(t => t.UserId);

        // ============================================
        // Custom Pages
        // ============================================
        modelBuilder.Entity<CustomPage>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<CustomPage>()
            .HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Применяет snake_case именование для PostgreSQL
    /// </summary>
    private static void ApplySnakeCaseNaming(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Имя таблицы
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                entity.SetTableName(ToSnakeCase(tableName));
            }

            // Имена колонок
            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (!string.IsNullOrEmpty(columnName))
                {
                    property.SetColumnName(ToSnakeCase(columnName));
                }
            }

            // Имена ключей
            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName))
                {
                    key.SetName(ToSnakeCase(keyName));
                }
            }

            // Имена FK
            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (!string.IsNullOrEmpty(fkName))
                {
                    fk.SetConstraintName(ToSnakeCase(fkName));
                }
            }

            // Имена индексов
            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexName))
                {
                    index.SetDatabaseName(ToSnakeCase(indexName));
                }
            }
        }
    }

    /// <summary>
    /// Конвертирует PascalCase/camelCase в snake_case
    /// </summary>
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        result.Append(char.ToLowerInvariant(input[0]));

        for (int i = 1; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                result.Append('_');
                result.Append(char.ToLowerInvariant(c));
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}