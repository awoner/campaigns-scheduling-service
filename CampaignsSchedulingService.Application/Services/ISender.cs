namespace CampaignsSchedulingService.Api.BackgroundJobs;

public interface ISender<T>
{
    public Task SendAsync(IEnumerable<T> items, CancellationToken cancellationToken);
}