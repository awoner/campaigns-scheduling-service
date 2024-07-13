namespace CampaignsSchedulingService.Domain;

public interface IScheduledItem
{
    DateTimeOffset ScheduledDate { get; }
}