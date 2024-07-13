using CampaignsSchedulingService.Domain;

namespace CampaignsSchedulingService.Infrastructure.Scheduling;

public interface ISchedulingProcessor<T> where T : IScheduledItem
{
    public Task<IEnumerable<T>> GetScheduledItemsAsync(CancellationToken cancellationToken);
}