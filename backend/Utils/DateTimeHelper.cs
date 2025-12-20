using System;

namespace backend.Utils
{
    public static class DateTimeHelper
    {
        public static DateTime GetServerLocalTime()
        {
            return DateTime.Now;
        }

        public static DateTimeOffset GetServerLocalDateTimeOffset()
        {
            return DateTimeOffset.Now;
        }

        public static int GetServerTimezoneOffsetMinutes()
        {
            return (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
        }

        public static string GetServerTimezoneId()
        {
            try
            {
                return TimeZoneInfo.Local.Id;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
