using CampaignsSchedulingService.Domain;
using CampaignsSchedulingService.Infrastructure.Scheduling.Helpers;

namespace CampaignsSchedulingService.Infrastructure.Scheduling;

public abstract class BaseSchedulingProcessor<T> : ISchedulingProcessor<T>
    where T : IScheduledItem
{
    protected abstract Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken);

    public async Task<IEnumerable<T>> GetScheduledItemsAsync(CancellationToken cancellationToken)
    {
        var items = await GetAsync(cancellationToken);
        var now = DateTimeOffset.UtcNow.Trim(DateTimeOffsetExtensions.DateTimePrecision.Minute);

        return items.Where(r => r.ScheduledDate == now);
    }
}