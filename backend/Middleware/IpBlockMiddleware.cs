using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using backend.Data;
using backend.Utils;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace backend.Middleware;

public class IpBlockMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY_PREFIX = "blocked_ip_";
    private static readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(5);

    public IpBlockMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        var remoteIp = IpAddressHelper.GetClientIpAddress(context);

        if (!string.IsNullOrEmpty(remoteIp))
        {
            var cacheKey = $"{CACHE_KEY_PREFIX}{remoteIp}";
            
            if (!_cache.TryGetValue(cacheKey, out bool isBlocked))
            {
                isBlocked = await dbContext.BlockedIps
                    .AnyAsync(b => b.IpAddress == remoteIp);

                _cache.Set(cacheKey, isBlocked, CACHE_DURATION);
            }

            if (isBlocked)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json; charset=utf-8";
                
                var response = new
                {
                    error = "access_denied",
                    message = "Ваш аккаунт заблокирован на данном ресурсе",
                    blocked = true
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response, new JsonSerializerOptions 
                    { 
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                    })
                );
                return;
            }
        }

        await _next(context);
    }

    public static void ClearIpCache(IMemoryCache cache, string ipAddress)
    {
        var cacheKey = $"{CACHE_KEY_PREFIX}{ipAddress}";
        cache.Remove(cacheKey);
    }
}
