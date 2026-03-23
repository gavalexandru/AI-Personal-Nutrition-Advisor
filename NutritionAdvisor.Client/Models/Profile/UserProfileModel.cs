using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Client.Models.Profile;

public class UserProfileModel
{
    public double HeightCm { get; set; }
    public double WeightKg { get; set; }
    public ActivityLevel ActivityLevel { get; set; } 
    public GoalType Goal { get; set; } 
    public double DailyCalorieTarget { get; set; }
}