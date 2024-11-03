namespace AppCore.Extensions;

public static class DatetimeExtension
{
    public static long TotalSeconds(this DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime);
        return dateTimeOffset.ToUnixTimeSeconds();
    }

    public static DateTime UtcNow()
    {
        return DateTime.Now;
    }

    public static DateTimeOffset Now(string timeZoneId)
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, timeZoneId);
    }

    public static DateTime StartOfMonth(this DateTime dt)
    {
        return dt.AddDays(1 - dt.Day);
    }

    public static DateTime EndOfMonth(this DateTime dateTime)
    {
        return dateTime.AddDays(DateTime.DaysInMonth(dateTime.Year, dateTime.Month) + 1 - dateTime.Day)
            .AddMilliseconds(-1);
    }

    public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        var diff = dateTime.DayOfWeek - startOfWeek;
        if (diff < 0) diff += 7;

        return dateTime.AddDays(-1 * diff).Date;
    }

    public static DateTime EndOfWeek(this DateTime dateTime, DayOfWeek endOfWeek = DayOfWeek.Sunday)
    {
        if (dateTime.DayOfWeek == endOfWeek)
        {
            return dateTime.Date.Date.AddDays(1).AddMilliseconds(-1);
        }

        var diff = dateTime.DayOfWeek - endOfWeek;
        return dateTime.AddDays(7 - diff).Date.AddDays(1).AddMilliseconds(-1);
    }
    
    
    public static int GetAge(this DateTime dateOfBirth)
    {
        var today = DateTime.Today;

        var a = (today.Year * 100 + today.Month) * 100 + today.Day;
        var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

        return (a - b) / 10000;
    }
}