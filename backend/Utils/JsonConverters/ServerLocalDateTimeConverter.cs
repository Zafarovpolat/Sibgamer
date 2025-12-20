using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Utils.JsonConverters
{
    public class ServerLocalDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrEmpty(s)) return default;

                if (s.IndexOf('Z', StringComparison.OrdinalIgnoreCase) >= 0 || System.Text.RegularExpressions.Regex.IsMatch(s, "[+-]\\d{2}:?\\d{2}$"))
                {
                    if (DateTimeOffset.TryParse(s, out var dto))
                    {
                        var local = TimeZoneInfo.ConvertTime(dto, TimeZoneInfo.Local).DateTime;
                        if (local.Kind == DateTimeKind.Unspecified)
                            local = DateTime.SpecifyKind(local, DateTimeKind.Local);
                        return local;
                    }
                }

                if (DateTime.TryParse(s, out var dt))
                {
                    if (dt.Kind == DateTimeKind.Unspecified)
                        dt = DateTime.SpecifyKind(dt, DateTimeKind.Local);
                    return dt.ToLocalTime();
                }
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            DateTime local = value.Kind switch
            {
                DateTimeKind.Utc => value.ToLocalTime(),
                DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Local),
                _ => value
            };

            var offset = TimeZoneInfo.Local.GetUtcOffset(local);
            var dto = new DateTimeOffset(local, offset);
            writer.WriteStringValue(dto.ToString("o"));
        }
    }

    public class ServerLocalNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly ServerLocalDateTimeConverter _inner = new ServerLocalDateTimeConverter();

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            return _inner.Read(ref reader, typeof(DateTime), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            _inner.Write(writer, value.Value, options);
        }
    }
}
