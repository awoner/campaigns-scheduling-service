using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Convertors;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

public class InMemoryRepository<TEntity, TDal> : IRepository<TEntity>
    where TEntity : IEntity
    where TDal : BaseDal
{
    private readonly IConvertor<TEntity, TDal> _convertor;
    private readonly List<TDal> _items = new List<TDal>();

    public InMemoryRepository(IConvertor<TEntity, TDal> convertor)
    {
        _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
    }
    
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _items.Add(_convertor.FromDomain(entity));

        return Task.CompletedTask;
    }

    public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_convertor.ToDomain(_items.Single(i => i.Id == id)));
    }

    public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_items.Where(d => predicate(_convertor.ToDomain(d))).Select(d => _convertor.ToDomain(d)));
    }

    public Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_items.Select(d => _convertor.ToDomain(d)).AsEnumerable());
    }
}