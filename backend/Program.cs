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
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
});

// ============================================
// Database Configuration (PostgreSQL/MySQL)
// ============================================
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string not configured. Set DATABASE_URL environment variable or DefaultConnection in appsettings.json");
}

// Определяем тип базы данных
var usePostgres = connectionString.StartsWith("postgres://") 
    || connectionString.StartsWith("postgresql://")
    || Environment.GetEnvironmentVariable("USE_POSTGRES") == "true";

if (usePostgres)
{
    // Конвертируем Supabase/Heroku URL в Npgsql формат
    if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
    {
        connectionString = ConvertPostgresUrl(connectionString);
    }
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
    
    Console.WriteLine("Using PostgreSQL database");
}
else
{
    // MySQL (оригинальная конфигурация)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));
    
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
// Background Services
// ============================================
builder.Services.AddHostedService<ServerMonitoringService>();
builder.Services.AddHostedService<PrivilegeExpirationService>();
builder.Services.AddHostedService<PendingPaymentCancellationService>();
builder.Services.AddHostedService<AdminSyncBackgroundService>();
builder.Services.AddHostedService<VipSyncBackgroundService>();
builder.Services.AddHostedService<TelegramBotBackgroundService>();
builder.Services.AddHostedService<EventNotificationBackgroundService>();

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
    throw new InvalidOperationException("JWT_KEY must be at least 32 characters. Set JWT_KEY environment variable or Jwt:Key in appsettings.json");
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
// CORS
// ============================================
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? builder.Configuration["FrontendUrl"]
    ?? "http://localhost:5173";

var allowedOrigins = new List<string> { frontendUrl };

// Добавляем www версию если https
if (frontendUrl.StartsWith("https://") && !frontendUrl.Contains("www."))
{
    allowedOrigins.Add(frontendUrl.Replace("https://", "https://www."));
}

// Добавляем localhost для разработки
if (!frontendUrl.Contains("localhost"))
{
    allowedOrigins.Add("http://localhost:5173");
    allowedOrigins.Add("http://localhost:3000");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
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

// Swagger в development и production (для тестирования)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseStaticFiles();

app.UseMiddleware<backend.Middleware.IpBlockMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ============================================
// Database Connection Check
// ============================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Проверка подключения к базе данных...");

        var canConnect = await context.Database.CanConnectAsync();
        if (canConnect)
        {
            logger.LogInformation("Подключение к базе данных успешно.");
        }
        else
        {
            logger.LogWarning("Не удалось подключиться к базе данных.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при подключении к базе данных.");
    }
}

app.Run();

// ============================================
// Helper Functions
// ============================================

/// <summary>
/// Конвертация PostgreSQL URL (Supabase/Heroku формат) в Npgsql connection string
/// postgres://user:password@host:port/database -> Host=...;Port=...;Database=...;Username=...;Password=...
/// </summary>
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

        // Supabase требует SSL
        var sslMode = "Require";
        
        // Проверяем query параметры для дополнительных настроек
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        if (query["sslmode"] != null)
        {
            sslMode = query["sslmode"];
        }

        return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode={sslMode};Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"Failed to parse DATABASE_URL: {ex.Message}", ex);
    }
}