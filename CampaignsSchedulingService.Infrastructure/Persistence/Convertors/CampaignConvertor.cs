using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Convertors;

public class CampaignConvertor : IConvertor<Campaign, CampaignDal>
{
    public CampaignDal FromDomain(Campaign domain)
    {
        return new CampaignDal
        {
            Id = domain.Id,
            Template = domain.Template,
            Priority = domain.Priority,
            ScheduledDate = domain.ScheduledDate,
        };
    }

    public Campaign ToDomain(CampaignDal dal)
    {
        return new Campaign(dal.Id, dal.Template, dal.Priority, dal.ScheduledDate);
    }
}