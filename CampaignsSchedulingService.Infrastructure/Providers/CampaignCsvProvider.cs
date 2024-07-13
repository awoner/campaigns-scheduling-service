using CampaignsSchedulingService.Infrastructure.Dals;

namespace CampaignsSchedulingService.Infrastructure.Providers;

public class CampaignCsvProvider : BaseCsvFileProvider<CampaignDal>
{
    private const string FileName = "Resources/campaigns.csv";
    private const string Delimiter = ",";

    public CampaignCsvProvider() : base(FileName, Delimiter)
    {
    }
}