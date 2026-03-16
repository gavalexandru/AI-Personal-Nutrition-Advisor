using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

public record UserProfileRequest(
    double HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    GoalType Goal,
    double DailyCalorieTarget);
