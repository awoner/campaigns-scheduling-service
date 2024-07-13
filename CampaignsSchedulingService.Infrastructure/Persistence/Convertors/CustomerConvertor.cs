using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;

namespace CampaignsSchedulingService.Infrastructure.Persistence.Convertors;

public class CustomerConvertor : IConvertor<Customer, CustomerDal>
{
    public CustomerDal FromDomain(Customer domain)
    {
        return new CustomerDal
        {
            Id = domain.Id,
            Age = domain.Age,
            City = domain.City,
            Deposit = domain.Deposit,
            Gender = domain.Gender switch
            {
                Gender.Male => GenderDal.Male,
                Gender.Female => GenderDal.Female,
            },
            IsNewCustomer = domain.IsNewCustomer
        };
    }

    public Customer ToDomain(CustomerDal dal)
    {
        var gender = dal.Gender switch {
            GenderDal.Male => Gender.Male,
            GenderDal.Female => Gender.Female,
        };

        return new Customer(dal.Id, dal.Age, gender, dal.City, dal.Deposit, dal.IsNewCustomer);
    }
}