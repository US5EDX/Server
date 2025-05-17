namespace Server.Services.Converters;

public static class KiyvDateTimeConverter
{
    private readonly static TimeZoneInfo _kiyvTimeZone;

    static KiyvDateTimeConverter() => _kiyvTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

    public static DateOnly ConvertUtcToKyivDate(DateTime utcDateTime)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Date must be in UTC format");

        return DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(utcDateTime, _kiyvTimeZone));
    }
}
