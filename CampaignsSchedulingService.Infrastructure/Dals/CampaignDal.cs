using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Providers;

namespace CampaignsSchedulingService.Infrastructure.Dals;

public class CampaignDal : BaseDal, ICsvRecord
{
    public int Id { get; set; }
    
    public string Template { get; set; }
    

    public Func<Customer, bool> Condition { get; set; }

    public int Priority { get; set; }
    
    public DateTimeOffset ScheduledDate { get; set; }
}