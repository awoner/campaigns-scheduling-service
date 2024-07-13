using CampaignsSchedulingService.Domain.Entities;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Convertors;

public interface IConvertor<TEntity, TDal>
    where TEntity : IEntity
{
    TDal FromDomain(TEntity domain);

    TEntity ToDomain(TDal dal);
}