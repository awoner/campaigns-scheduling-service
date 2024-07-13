using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Convertors;
using CampaignsSchedulingService.Infrastructure.Providers;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Repositories;

public class CustomerCsvRepository : CsvRepository<Customer, CustomerDal>
{
    public CustomerCsvRepository(ICsvFileProvider<CustomerDal> csvFileProvider, IConvertor<Customer, CustomerDal> convertor)
        : base(csvFileProvider, convertor)
    {
    }
}