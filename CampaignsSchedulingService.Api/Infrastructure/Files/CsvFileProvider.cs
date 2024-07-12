using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CampaignsSchedulingService.Api.Infrastructure.Files;

public abstract class Record
{
    public int Id { get; set; }
}

public class CsvFileProvider<TRecord> where TRecord : Record
{
    private readonly IFileProvider _fileProvider;

    private readonly string _fileName;
    private readonly string _delimiter;

    private IReadOnlyCollection<TRecord> _records;

    protected IReadOnlyCollection<TRecord> Records => _records ?? (_records = Load());

    protected CsvFileProvider(string fileName, string delimiter)
        : this(fileName, delimiter, new EmbeddedFileProvider(typeof(TRecord).Assembly))
    {
    }

    protected CsvFileProvider(string fileName, string delimiter, IFileProvider fileProvider)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        _delimiter = delimiter;

        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    protected IReadOnlyCollection<TRecord> Load()
    {
        using (var reader = new StreamReader(_fileProvider.GetFileInfo(_fileName).CreateReadStream()))
        using (var csv = new CsvReader(reader, new CsvConfiguration (CultureInfo.InvariantCulture) { Delimiter = _delimiter }))
        {
            return csv.GetRecords<TRecord>().ToList();
        }
    }

    public virtual IEnumerable<TRecord> Get() => Records;

    public bool Has(int id) => Get().Any(c => c.Id == id);

    public TRecord SingleOrDefault(int id) => Get().SingleOrDefault(c => c.Id == id);

    public TRecord Single(int id) => 
        SingleOrDefault(id) ?? throw new Exception($"Record of type {typeof(TRecord)} with id {id} not found.");
}