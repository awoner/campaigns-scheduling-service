namespace CampaignsSchedulingService.Infrastructure.Providers;

public interface ICsvFileProvider<TRecord> where TRecord : ICsvRecord
{
    IEnumerable<TRecord> Get();

    IEnumerable<TRecord> Get(Func<TRecord, bool> predicate);
    
    TRecord SingleOrDefault(int id);

    TRecord Single(int id);
}