using CampaignsSchedulingService.Infrastructure.Dals;

namespace CampaignsSchedulingService.Infrastructure.Providers;

public class CustomerCsvProvider : BaseCsvFileProvider<CustomerDal>
{
    private const string FileName = "Resources/customers.csv";
    private const string Delimiter = ",";

    public CustomerCsvProvider() : base(FileName, Delimiter)
    {
    }
}