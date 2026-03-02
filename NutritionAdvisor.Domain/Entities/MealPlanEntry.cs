using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class MealPlanEntry
{
    public Guid Id { get; private set; }
    public MealPlan MealPlan { get; private set; } = null!;

    public DateTime Date { get; private set; }
    public MealType MealType { get; private set; }

    public Recipe Recipe { get; private set; } = null!;
}