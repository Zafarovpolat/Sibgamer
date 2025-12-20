using Microsoft.AspNetCore.Http;

namespace backend.Utils;

public static class IpAddressHelper
{
    public static string? GetClientIpAddress(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
        {
            var ip = realIp.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ip) && IsValidIpAddress(ip))
            {
                return ip;
            }
        }

        if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
        {
            var ip = cfIp.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ip) && IsValidIpAddress(ip))
            {
                return ip;
            }
        }

        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ips = forwardedFor.FirstOrDefault()?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (ips != null && ips.Length > 0)
            {
                var ip = ips[0].Trim();
                if (!string.IsNullOrWhiteSpace(ip) && IsValidIpAddress(ip))
                {
                    return ip;
                }
            }
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private static bool IsValidIpAddress(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return false;

        var ipWithoutPort = ip.Split(':')[0];

        return ipWithoutPort.Contains('.') || ipWithoutPort.Contains(':');
    }
}
