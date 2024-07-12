using CampaignsSchedulingService.Api.Entities;

namespace CampaignsSchedulingService.Api.Infrastructure;

public static class CampaignStorage
{
    public static List<Campaign> Campaigns = new List<Campaign>();
    public static Dictionary<int, DateTime> CustomerLastCampaign = new Dictionary<int, DateTime>();
}

public class CustomersRepository
{
    public async Task<Customer> GetCustomerAsync(int id, CancellationToken cancellationToken)
    {
    }
}