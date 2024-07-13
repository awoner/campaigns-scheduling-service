using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Convertors;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

public class CampaignInMemoryRepository : InMemoryRepository<Campaign, CampaignDal>
{
    public CampaignInMemoryRepository(IConvertor<Campaign, CampaignDal> convertor) : base(convertor)
    {
    }
}