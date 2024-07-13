namespace CampaignsSchedulingService.Infrastructure.Scheduling.Helpers;

public static class DateTimeOffsetExtensions
{
    public enum DateTimePrecision
    {
        Millisecond,
        Second,
        Minute,
        Hour,
        Day,
    }
    
    public static DateTimeOffset Trim(this DateTimeOffset dateTimeOffset, DateTimePrecision precision)
    {
        var ticks = precision switch
        {
            DateTimePrecision.Millisecond => TimeSpan.TicksPerMillisecond,
            DateTimePrecision.Second => TimeSpan.TicksPerSecond,
            DateTimePrecision.Minute => TimeSpan.TicksPerMinute,
            DateTimePrecision.Hour => TimeSpan.TicksPerHour,
            DateTimePrecision.Day => TimeSpan.TicksPerDay,
            _ => throw new ArgumentOutOfRangeException(nameof(precision), precision, null)
        };

        return new DateTimeOffset(new DateTime(dateTimeOffset.Ticks - (dateTimeOffset.Ticks % ticks)), dateTimeOffset.Offset);
    }
}