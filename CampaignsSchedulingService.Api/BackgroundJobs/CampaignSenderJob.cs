using System.Runtime.CompilerServices;
using CampaignsSchedulingService.Api.Entities;

namespace CampaignsSchedulingService.Api.BackgroundJobs;

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

    public static int DaysDifferenceFrom(this DateTimeOffset first, DateTimeOffset second)
    {
        return first.Subtract(second).Days;
    }

    public static DateTimeOffset GetEndOfMonth(this DateTimeOffset now)
    {
        return new DateTimeOffset(new DateTime(now.Year, now.Month, 1).AddMonths(1).AddSeconds(-1), TimeSpan.Zero);
    }

    public static bool Includes(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)
    {
        return start1 <= end2 && start2 <= end1;
    }
}

interface ISchedulingProcessor
{
    
}

public class CampaignSchedulingProcessor : ISchedulingProcessor
{
    private readonly List<Customer> _custeomersWithCampainsToday;
    private readonly List<Campaign> _campaigns;
    private readonly List<Customer> _customers;

    public CampaignSchedulingProcessor()
    {
        _campaigns = new List<Campaign>();
        _customers = new List<Customer>();
    }
    
    public async Task SendScheduledCampaignsAsync()
    {
        // TODO:
        // 1. Get actual campaigns that need to send now
        // 2. Get target customers for each campaign (target means that customer need to pass campaign condition and 
        // 3. Send campaign with target customers
        
        var actualCampaigns = GetActualCampaigns();
        var customers = GetTargetCustomers(actualCampaigns);
        
        var campaignCustomersDict = new Dictionary<Campaign, List<Customer>>();
        foreach (var customer in customers)
        {
            var customerCampaigns = actualCampaigns.Where(c => c.Condition(customer));
            var priorityCustomerCampaign = customerCampaigns.MaxBy(c => c.Priority);

            if (campaignCustomersDict.TryGetValue(priorityCustomerCampaign, out var campaignCustomers))
                campaignCustomers.Add(customer);
            else
                campaignCustomersDict.TryAdd(priorityCustomerCampaign, [customer]);
        }
        
        foreach (var (campaign, campaignCustomers) in campaignCustomersDict)
        {
            await SendAsync(campaign);
        }
    }

    private IEnumerable<Customer> GetTargetCustomers(IEnumerable<Campaign> actualCampaigns)
    {
        return _customers.Where(customer => actualCampaigns.Any(c => c.Condition(customer)));
    }

    private IEnumerable<Campaign> GetActualCampaigns()
    {
        var campaigns = _campaigns;
        
        var now = DateTimeOffset.UtcNow.Trim(DateTimeOffsetExtensions.DateTimePrecision.Minute);
        return campaigns.Where(r => r.ScheduledDate == now);
    }

    private static async Task SendAsync(Campaign campaign)
    {
        var fileName = $"sends{campaign.ScheduledDate:yyyyMMdd}.txt";
        await using (var writer = new StreamWriter(fileName, true))
        {
            await writer.WriteLineAsync(campaign.Template);
        }

        await Task.Delay(TimeSpan.FromMinutes(30));
    }
}

public class CampaignSenderJob : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}