using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.Queries.GetProfile;

public record UserProfileDto(
    double HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    GoalType Goal,
    double DailyCalorieTarget);