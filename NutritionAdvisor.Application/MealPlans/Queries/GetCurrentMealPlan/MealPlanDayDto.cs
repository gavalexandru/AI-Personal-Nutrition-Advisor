namespace NutritionAdvisor.Application.MealPlans.Queries.GetCurrentMealPlan;

public class MealPlanDayDto
{
    public int Day { get; set; }
    public List<MealDto> Meals { get; set; } = [];
}

public class MealDto
{
    public string MealType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
}