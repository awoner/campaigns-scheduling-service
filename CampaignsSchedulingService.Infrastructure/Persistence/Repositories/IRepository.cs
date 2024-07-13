using CampaignsSchedulingService.Domain.Entities;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task<TEntity> GetAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken);
    
    Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken);
}