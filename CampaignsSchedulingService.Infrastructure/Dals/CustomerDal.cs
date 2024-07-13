using CampaignsSchedulingService.Infrastructure.Providers;

namespace CampaignsSchedulingService.Infrastructure.Dals;

public class CustomerDal : BaseDal, ICsvRecord
{
    public int Age { get; set; }

    public GenderDal Gender { get; set; }

    public string City { get; set; }
    
    public int Deposit { get; set; }

    public bool IsNewCustomer { get; set; }
}

public enum GenderDal
{
    Male = 1,
    Female = 2,
}