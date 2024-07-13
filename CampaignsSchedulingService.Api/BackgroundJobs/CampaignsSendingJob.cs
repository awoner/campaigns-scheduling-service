
using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Scheduling;

namespace CampaignsSchedulingService.Api.BackgroundJobs;

public class CampaignsSendingJob : BackgroundService
{
    private readonly ISchedulingProcessor<Campaign> _schedulingProcessor;
    private readonly ISender<Campaign> _sender;

    public CampaignsSendingJob(ISchedulingProcessor<Campaign> schedulingProcessor, ISender<Campaign> sender)
    {
        _schedulingProcessor = schedulingProcessor ?? throw new ArgumentNullException(nameof(schedulingProcessor));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            var actualItems = await _schedulingProcessor.GetScheduledItemsAsync(stoppingToken);
            await _sender.SendAsync(actualItems, stoppingToken);
            
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}