using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public double HeightCm { get; private set; }
    public double WeightKg { get; private set; }
    public ActivityLevel ActivityLevel { get; private set; }
    public GoalType Goal { get; private set; } // LoseWeight, Maintain, Gain

    public double DailyCalorieTarget { get; private set; }
}