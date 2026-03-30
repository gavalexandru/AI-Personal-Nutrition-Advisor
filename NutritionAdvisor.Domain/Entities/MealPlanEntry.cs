using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class MealPlanEntry(MealPlan mealPlan, DateTime date, MealType mealType, Recipe recipe)
{
    private MealPlanEntry() : this(null!, default, default, null!) { }
    public Guid Id { get; private set; } = Guid.NewGuid();
    public MealPlan MealPlan { get; private set; } = mealPlan;

    public DateTime Date { get; private set; } = date;
    public MealType MealType { get; private set; } = mealType;

    public Recipe Recipe { get; private set; } = recipe;
}