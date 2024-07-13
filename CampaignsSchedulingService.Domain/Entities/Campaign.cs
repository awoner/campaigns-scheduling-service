namespace CampaignsSchedulingService.Domain.Entities;

public class Campaign : IEntity, IScheduledItem
{
    public Campaign(int id, string template, int priority, DateTimeOffset scheduledDate)
    {
        Id = id;
        Template = template;
        Priority = priority;
        ScheduledDate = scheduledDate;
        Condition = _ => true;
    }
    
    public int Id { get; private set; }
    
    public string Template { get; private set; }

    public Func<Customer, bool> Condition { get; private set; }

    public int Priority { get; private set; }
    
    public DateTimeOffset ScheduledDate { get; private set; }

    public void SetCondition(Func<Customer, bool> condition)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
    }
}