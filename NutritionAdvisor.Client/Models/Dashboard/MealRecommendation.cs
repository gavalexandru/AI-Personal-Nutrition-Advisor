namespace NutritionAdvisor.Client.Models.Dashboard;

public class MealRecommendation
{
    public string Type { get; set; } = string.Empty; 
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Calories { get; set; }
}