using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Repositories;
using CampaignsSchedulingService.Infrastructure.Providers;

namespace CampaignsSchedulingService.Api.BackgroundJobs;

public class CampaignsDataLoaderJob : BackgroundService
{
    private readonly IRepository<Campaign> _repository;
    private readonly ICsvFileProvider<CampaignDal> _csvFileProvider;

    public CampaignsDataLoaderJob(IRepository<Campaign> repository, ICsvFileProvider<CampaignDal> csvFileProvider)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _csvFileProvider = csvFileProvider ?? throw new ArgumentNullException(nameof(csvFileProvider));
    }
    
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var campaignDals = _csvFileProvider.Get();
        var templates = await GetTemplatesAsync(stoppingToken);
        
        foreach (var campaignDal in campaignDals)
        {
            var template = templates[campaignDal.Template];
            Func<Customer, bool> condition = campaignDal.Id switch
            {
                1 => c => c.Gender == Gender.Male,
                2 => c => c.Age > 45,
                3 => c => c.City == "New York",
                4 => c => c.Deposit > 100,
                5 => c => c.IsNewCustomer,
                _ => _ => true
            };

            var campaign = new Campaign(campaignDal.Id, template, campaignDal.Priority, campaignDal.ScheduledDate);
            campaign.SetCondition(condition);

            await _repository.AddAsync(campaign, stoppingToken);
        }
    }

    private async Task<Dictionary<string, string>> GetTemplatesAsync(CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, string>();
        
        var templateNames = new [] {"TemplateA", "TemplateB", "TemplateC"};
        foreach (var templateName in templateNames)
        {
            result.Add(templateName, await GetTemplateAsync(templateName, cancellationToken));
        }

        return result;
    }

    private async Task<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        var fileName = $"Resources/{templateName}.html";
        using var reader = new StreamReader(fileName, true);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}