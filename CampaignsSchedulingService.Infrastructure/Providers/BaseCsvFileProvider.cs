using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.FileProviders;

namespace CampaignsSchedulingService.Infrastructure.Providers;

public abstract class BaseCsvFileProvider<TRecord> : ICsvFileProvider<TRecord> where TRecord : ICsvRecord
{
    private readonly IFileProvider _fileProvider;

    private readonly string _fileName;
    private readonly string _delimiter;

    private IReadOnlyCollection<TRecord> _records;

    protected IReadOnlyCollection<TRecord> Records => _records ?? (_records = Load());

    protected BaseCsvFileProvider(string fileName, string delimiter)
        : this(fileName, delimiter, new EmbeddedFileProvider(typeof(TRecord).Assembly))
    {
    }

    protected BaseCsvFileProvider(string fileName, string delimiter, IFileProvider fileProvider)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        _delimiter = delimiter;

        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    protected IReadOnlyCollection<TRecord> Load()
    {
        using var reader = new StreamReader(_fileProvider.GetFileInfo(_fileName).CreateReadStream());
        using var csv = new CsvReader(reader, new CsvConfiguration (CultureInfo.InvariantCulture) { Delimiter = _delimiter });

        return csv.GetRecords<TRecord>().ToList();
    }

    public IEnumerable<TRecord> Get() => Records;

    public IEnumerable<TRecord> Get(Func<TRecord, bool> predicate)
    {
        return Get().Where(predicate);
    }

    public TRecord SingleOrDefault(int id) => Get().SingleOrDefault(c => c.Id == id);

    public TRecord Single(int id) => 
        SingleOrDefault(id) ?? throw new Exception($"Record of type {typeof(TRecord)} with id {id} not found.");
}