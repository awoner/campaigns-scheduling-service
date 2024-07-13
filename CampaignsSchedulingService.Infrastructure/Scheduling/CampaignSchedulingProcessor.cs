using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

namespace CampaignsSchedulingService.Infrastructure.Scheduling;

public class CampaignSchedulingProcessor : BaseSchedulingProcessor<Campaign>
{
    private readonly IRepository<Campaign> _repository;

    public CampaignSchedulingProcessor(IRepository<Campaign> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    protected override Task<IEnumerable<Campaign>> GetAsync(CancellationToken cancellationToken)
    {
        return _repository.GetAsync(cancellationToken);
    }
}