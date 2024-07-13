using CampaignsSchedulingService.Infrastructure.Persistence;

namespace CampaignsSchedulingService.Api.BackgroundJobs;

public class DailyCustomerClearingJob : BackgroundService
{
    private Timer _timer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(ClearCustomerWithCampaigns, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private void ClearCustomerWithCampaigns(object _)
    {
        var currentTime = DateTime.Now;
        var nextMidnight = DateTime.Today.AddDays(1);

        if (currentTime < nextMidnight)
            return;

        CustomersWithCampaignsStorage.Data.Clear();
        _timer.Change(nextMidnight.AddSeconds(-currentTime.Second).Subtract(currentTime), TimeSpan.FromDays(1));
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        await base.StopAsync(stoppingToken);
    }
}