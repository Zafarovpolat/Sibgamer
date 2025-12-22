using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using backend.Data;
using backend.Services;
using backend.BackgroundServices;
using backend.Models;
using backend.Utils;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
});

// ============================================
// Database Configuration
// ============================================
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string not configured.");
}

var usePostgres = connectionString.StartsWith("postgres://") 
    || connectionString.StartsWith("postgresql://")
    || Environment.GetEnvironmentVariable("USE_POSTGRES") == "true";

if (usePostgres)
{
    if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
    {
        connectionString = ConvertPostgresUrl(connectionString);
    }
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(120);
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null
            );
        });
    });
    
    Console.WriteLine("Using PostgreSQL database with enhanced retry logic");
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)),
            mysqlOptions =>
            {
                mysqlOptions.CommandTimeout(120);
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            }));
    
    Console.WriteLine("Using MySQL database");
}

// ============================================
// Services
// ============================================
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IServerQueryService, ServerQueryService>();
builder.Services.AddScoped<IViewTrackingService, ViewTrackingService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISourceBansService, SourceBansService>();
builder.Services.AddScoped<IRconService, RconService>();
builder.Services.AddScoped<IVipService, VipService>();
builder.Services.AddScoped<IVipSyncService, VipSyncService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAdminSyncService, AdminSyncService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Services.AddHttpClient();

// ============================================
// Background Services - ОТКЛЮЧЕНЫ для диагностики
// ============================================
// Раскомментируйте после того как основной сайт заработает
// builder.Services.AddHostedService<ServerMonitoringService>();
// builder.Services.AddHostedService<PrivilegeExpirationService>();
// builder.Services.AddHostedService<PendingPaymentCancellationService>();
// builder.Services.AddHostedService<AdminSyncBackgroundService>();
// builder.Services.AddHostedService<VipSyncBackgroundService>();
// builder.Services.AddHostedService<TelegramBotBackgroundService>();
// builder.Services.AddHostedService<EventNotificationBackgroundService>();

// ============================================
// JWT Authentication
// ============================================
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? builder.Configuration["Jwt:Key"];
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? builder.Configuration["Jwt:Issuer"]
    ?? "SibGamer";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
    ?? builder.Configuration["Jwt:Audience"]
    ?? "SibGamerUsers";

if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException("JWT_KEY must be at least 32 characters.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    using var scope = context.HttpContext.RequestServices.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

                    var cacheKey = $"user_{userId}";
                    if (!cache.TryGetValue(cacheKey, out User? user))
                    {
                        user = await dbContext.Users.FindAsync(userId);
                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                        cache.Set(cacheKey, user, cacheOptions);
                    }

                    if (user == null || user.IsBlocked)
                    {
                        cache.Remove(cacheKey);
                        context.Fail("Пользователь не найден или заблокирован");
                        return;
                    }

                    context.HttpContext.Items["CurrentUser"] = user;
                }
                else
                {
                    context.Fail("Неверный токен");
                }
            }
        };
    });

builder.Services.AddAuthorization();

// ============================================
// CORS - Расширенная настройка
// ============================================
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? builder.Configuration["FrontendUrl"]
    ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                frontendUrl,
                frontendUrl.Replace("https://", "https://www."),
                "http://localhost:5173",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    
    // Добавляем политику для всех origins (для отладки)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ============================================
// Controllers & Swagger
// ============================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new backend.Utils.JsonConverters.ServerLocalDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new backend.Utils.JsonConverters.ServerLocalNullableDateTimeConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================
// Build App
// ============================================
var app = builder.Build();

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

app.UseSwagger();
app.UseSwaggerUI();

// Используем AllowAll для отладки - потом вернуть на AllowFrontend
app.UseCors("AllowAll");

app.UseStaticFiles();

// ОТКЛЮЧАЕМ IP блокировку для отладки
// app.UseMiddleware<backend.Middleware.IpBlockMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============================================
// Health Check Endpoint
// ============================================
app.MapGet("/health", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        return Results.Ok(new { 
            status = canConnect ? "healthy" : "unhealthy",
            database = canConnect ? "connected" : "disconnected",
            timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { 
            status = "unhealthy",
            database = "error",
            error = ex.Message,
            timestamp = DateTime.UtcNow
        });
    }
});

// ============================================
// Database Warm-up
// ============================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Прогрев базы данных...");
    
    for (int attempt = 1; attempt <= 5; attempt++)
    {
        try
        {
            // Простой запрос для "пробуждения" БД
            var canConnect = await context.Database.CanConnectAsync();
            if (canConnect)
            {
                // Делаем простой запрос чтобы соединение было "горячим"
                await context.Database.ExecuteSqlRawAsync("SELECT 1");
                logger.LogInformation("База данных готова (попытка {Attempt})", attempt);
                break;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning("Попытка {Attempt}/5 подключения к БД не удалась: {Error}", attempt, ex.Message);
            if (attempt < 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(10 * attempt));
            }
        }
    }
}

app.Run();

// ============================================
// Helper Functions
// ============================================
static string ConvertPostgresUrl(string url)
{
    try
    {
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':');
        var username = userInfo[0];
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');

        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var sslMode = query["sslmode"] ?? "Require";

        // Оптимизированные параметры для Supabase
        return $"Host={host};" +
               $"Port={port};" +
               $"Database={database};" +
               $"Username={username};" +
               $"Password={password};" +
               $"SSL Mode={sslMode};" +
               $"Trust Server Certificate=true;" +
               $"Pooling=true;" +
               $"Minimum Pool Size=0;" +
               $"Maximum Pool Size=5;" +        // Уменьшаем пул
               $"Connection Idle Lifetime=60;" + // Быстрее закрываем неиспользуемые
               $"Connection Pruning Interval=10;" +
               $"Keepalive=15;" +               // Чаще keepalive
               $"Timeout=120;" +                // Увеличенный таймаут
               $"Command Timeout=120;" +
               $"Internal Command Timeout=120";
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"Failed to parse DATABASE_URL: {ex.Message}", ex);
    }
}