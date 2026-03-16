using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class UserProfile(
    User user,
    double heightCm,
    double weightKg,
    ActivityLevel activityLevel,
    GoalType goal,
    double dailyCalorieTarget)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public User User { get; private set; } = user;

    public double HeightCm { get; private set; } = heightCm;
    public double WeightKg { get; private set; } = weightKg;
    public ActivityLevel ActivityLevel { get; private set; } = activityLevel;
    public GoalType Goal { get; private set; } = goal; // LoseWeight, Maintain, Gain

    public double DailyCalorieTarget { get; private set; } = dailyCalorieTarget;
    
    private UserProfile() : this(null!, 0, 0, default, default, 0) { }

    public void Update(double heightCm, double weightKg, ActivityLevel activityLevel, GoalType goal, double dailyCalorieTarget)
    {
        HeightCm = heightCm;
        WeightKg = weightKg;
        ActivityLevel = activityLevel;
        Goal = goal;
        DailyCalorieTarget = dailyCalorieTarget;
    }
}