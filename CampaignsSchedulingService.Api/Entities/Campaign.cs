namespace CampaignsSchedulingService.Api.Entities;

public class Campaign
{
    public string Template { get; set; }

    public Func<Customer, bool> Condition { get; set; }

    public DateTimeOffset ScheduledDate { get; set; }

    public int Priority { get; set; }
}

public class Customer
{
    public int Id { get; set; }

    public int Age { get; set; }

    public Gender Gender { get; set; }

    public string City { get; set; }
    
    public int Deposit { get; set; }

    public bool IsNewCustomer { get; set; }
}

public enum Gender
{
    Male = 1,
    Female = 2,
}
