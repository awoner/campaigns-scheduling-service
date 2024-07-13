namespace CampaignsSchedulingService.Domain.Entities;

public class Customer : IEntity
{
    public Customer(int id, int age, Gender gender, string city, int deposit, bool isNewCustomer)
    {
        Id = id;
        Age = age;
        Gender = gender;
        City = city;
        Deposit = deposit;
        IsNewCustomer = isNewCustomer;
    }
    
    public int Id { get; private set; }
    
    public int Age { get; private set; }

    public Gender Gender { get; private set; }

    public string City { get; private set; }
    
    public int Deposit { get; private set; }

    public bool IsNewCustomer { get; private set; }
}

public enum Gender
{
    Male = 1,
    Female = 2,
}