using CampaignsSchedulingService.Api.BackgroundJobs;
using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Persistence;
using CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

namespace CampaignsSchedulingService.Application.Services;

public class CampaignSender : ISender<Campaign>
{
    private readonly IRepository<Customer> _customerRepository;

    public CampaignSender(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }
    
    public async Task SendAsync(IEnumerable<Campaign> items, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetAsync(
            customer => items.Any(c => c.Condition(customer))
                        && !CustomersWithCampaignsStorage.Data.Contains(customer.Id), cancellationToken);;
        
        var campaignCustomersDict = new Dictionary<Campaign, List<Customer>>();
        foreach (var customer in customers)
        {
            var customerCampaigns = items.Where(c => c.Condition(customer));
            var priorityCustomerCampaign = customerCampaigns.MinBy(c => c.Priority);

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