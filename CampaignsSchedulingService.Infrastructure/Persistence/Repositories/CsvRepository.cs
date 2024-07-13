using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Convertors;
using CampaignsSchedulingService.Infrastructure.Providers;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

public class CsvRepository<TEntity, TDal> : IRepository<TEntity>
    where TEntity : IEntity
    where TDal : BaseDal, ICsvRecord
{
    private readonly ICsvFileProvider<TDal> _csvFileProvider;
    private readonly IConvertor<TEntity, TDal> _convertor;

    public CsvRepository(ICsvFileProvider<TDal> csvFileProvider, IConvertor<TEntity, TDal> convertor)
    {
        _csvFileProvider = csvFileProvider ?? throw new ArgumentNullException(nameof(csvFileProvider));
        _convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_convertor.ToDomain(_csvFileProvider.Single(id)));
    }

    public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_csvFileProvider.Get(d => predicate(_convertor.ToDomain(d))).Select(d => _convertor.ToDomain(d)));
    }

    public Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_csvFileProvider.Get().Select(e => _convertor.ToDomain(e)));
    }
}