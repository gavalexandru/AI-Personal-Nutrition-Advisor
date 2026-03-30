namespace NutritionAdvisor.Application.MealPlans.Models;

public class AiMealPlanRequest
{
    public double DailyCalorieTarget { get; set; }
    public List<string> Allergies { get; set; } = new();
    public List<string> DietPreferences { get; set; } = new();
    public int Days { get; set; }
    
    public string Goal { get; set; } = string.Empty;
    public string ActivityLevel { get; set; } = string.Empty;
}

public class AiMealPlanResponse
{
    public List<AiPlanDay> PlanDays { get; set; } = new();
}

public class AiPlanDay
{
    public int Day { get; set; }
    public List<AiMealEntry> Meals { get; set; } = new();
}

public class AiMealEntry
{
    public string MealType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
}