using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class MealPlan(User user, DateTime startDate, DateTime endDate, PlanType planType)
{
    private MealPlan() : this(null!, default, default, default) { }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public User User { get; private set; } = user;

    public DateTime StartDate { get; private set; } = startDate;
    public DateTime EndDate { get; private set; } = endDate;

    public PlanType PlanType { get; private set; } = planType; // Daily / Weekly
    public ICollection<MealPlanEntry> Entries { get; private set; } = new List<MealPlanEntry>();

    public void AddEntry(MealPlanEntry entry)
    {
        Entries.Add(entry);
    }
}