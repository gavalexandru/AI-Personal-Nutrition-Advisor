using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class MealPlan
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public PlanType PlanType { get; private set; } // Daily / Weekly
    public ICollection<MealPlanEntry> Entries { get; private set; } = null!;
}