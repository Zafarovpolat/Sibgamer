using System.Text.RegularExpressions;

namespace backend.Utils;

public static class SteamIdConverter
{
    private const long SteamId64Base = 76561197960265728L;
    
    public static (bool success, string? steamId, string? profileUrl, string? error) ConvertToSteamFormat(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (false, null, null, "Steam ID не может быть пустым");
        }

        input = input.Trim();

        try
        {
            if (input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return ParseSteamUrl(input);
            }

            var legacyMatch = Regex.Match(input, @"^STEAM_[01]:([01]):(\d+)$", RegexOptions.IgnoreCase);
            if (legacyMatch.Success)
            {
                var y = int.Parse(legacyMatch.Groups[1].Value);
                var z = long.Parse(legacyMatch.Groups[2].Value);
                var steam64 = SteamId64Base + (z * 2) + y;
                var profileUrl = $"https://steamcommunity.com/profiles/{steam64}";
                var normalizedSteamId = $"STEAM_0:{y}:{z}";
                return (true, normalizedSteamId, profileUrl, null);
            }

            var steam3Match = Regex.Match(input, @"^\[U:1:(\d+)\]$");
            if (steam3Match.Success)
            {
                var accountId = long.Parse(steam3Match.Groups[1].Value);
                return ConvertAccountIdToSteamFormat(accountId);
            }

            if (long.TryParse(input, out var steam64Id) && steam64Id > SteamId64Base)
            {
                return ConvertSteam64ToSteamFormat(steam64Id);
            }

            return (false, null, null, "Неверный формат Steam ID. Поддерживаются форматы: STEAM_0:0:X, [U:1:X], 76561198XXXXXXXXX или ссылка на профиль Steam");
        }
        catch (Exception ex)
        {
            return (false, null, null, $"Ошибка при обработке Steam ID: {ex.Message}");
        }
    }

    private static (bool success, string? steamId, string? profileUrl, string? error) ParseSteamUrl(string url)
    {
        try
        {
            var profileMatch = Regex.Match(url, @"steamcommunity\.com/profiles/(\d+)", RegexOptions.IgnoreCase);
            if (profileMatch.Success)
            {
                var steam64 = long.Parse(profileMatch.Groups[1].Value);
                return ConvertSteam64ToSteamFormat(steam64);
            }

            var customMatch = Regex.Match(url, @"steamcommunity\.com/id/([^/]+)", RegexOptions.IgnoreCase);
            if (customMatch.Success)
            {
                return (false, null, null, "Пожалуйста, используйте числовой Steam ID или ссылку с числовым профилем (steamcommunity.com/profiles/ЧИСЛА)");
            }

            return (false, null, null, "Не удалось извлечь Steam ID из URL");
        }
        catch (Exception ex)
        {
            return (false, null, null, $"Ошибка при парсинге URL: {ex.Message}");
        }
    }

    private static (bool success, string? steamId, string? profileUrl, string? error) ConvertSteam64ToSteamFormat(long steam64)
    {
        if (steam64 <= SteamId64Base)
        {
            return (false, null, null, "Неверный Steam64 ID");
        }

        var accountId = steam64 - SteamId64Base;
        return ConvertAccountIdToSteamFormat(accountId);
    }

    private static (bool success, string? steamId, string? profileUrl, string? error) ConvertAccountIdToSteamFormat(long accountId)
    {
        var y = accountId % 2;
        var z = (accountId - y) / 2;
        var steamId = $"STEAM_0:{y}:{z}";
        var steam64 = SteamId64Base + accountId;
        var profileUrl = $"https://steamcommunity.com/profiles/{steam64}";

        return (true, steamId, profileUrl, null);
    }

    public static bool IsValidSteamIdFormat(string? steamId)
    {
        if (string.IsNullOrWhiteSpace(steamId))
            return false;

        return Regex.IsMatch(steamId, @"^STEAM_0:[01]:\d+$", RegexOptions.IgnoreCase);
    }

    public static long? ConvertToSteam64(string steamId)
    {
        if (!IsValidSteamIdFormat(steamId))
            return null;

        var match = Regex.Match(steamId, @"^STEAM_0:([01]):(\d+)$", RegexOptions.IgnoreCase);
        if (!match.Success)
            return null;

        var y = int.Parse(match.Groups[1].Value);
        var z = long.Parse(match.Groups[2].Value);
        return SteamId64Base + (z * 2) + y;
    }
}
