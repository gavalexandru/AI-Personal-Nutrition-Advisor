using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class Subscription
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public SubscriptionType Type { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public bool IsActive { get; private set; }
    
    private Subscription() { }
    
    public Subscription(User user)
    {
        User = user;
        Type = SubscriptionType.Free;
        IsActive = false;
        StartDate = DateTime.UtcNow;
        EndDate = null;
    }
    
}