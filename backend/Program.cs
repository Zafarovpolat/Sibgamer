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
using MySqlConnector;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));


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

builder.Services.AddHostedService<ServerMonitoringService>();
builder.Services.AddHostedService<PrivilegeExpirationService>();
builder.Services.AddHostedService<PendingPaymentCancellationService>();
builder.Services.AddHostedService<AdminSyncBackgroundService>();
builder.Services.AddHostedService<VipSyncBackgroundService>();
builder.Services.AddHostedService<TelegramBotBackgroundService>();
builder.Services.AddHostedService<EventNotificationBackgroundService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
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

var frontendUrl = builder.Configuration["FrontendUrl"] ?? "https://sibgamer.com";
var frontendUrlWww = frontendUrl.StartsWith("https://") ? frontendUrl.Replace("https://", "https://www.") : frontendUrl;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
          policy.WithOrigins(frontendUrl, frontendUrlWww)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new backend.Utils.JsonConverters.ServerLocalDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new backend.Utils.JsonConverters.ServerLocalNullableDateTimeConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseStaticFiles();

app.UseMiddleware<backend.Middleware.IpBlockMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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
            logger.LogWarning("Не удалось подключиться к базе данных. Убедитесь, что база существует и выполните backend/db/schema.sql для создания структуры.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при подключении к базе данных. Убедитесь, что настройки подключения верны и схема базы импортирована (backend/db/schema.sql).");
    }
}

app.Run();