using System.Text.Json.Serialization;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

public record UserProfileRequest(
    [property: JsonRequired] double HeightCm,
    [property: JsonRequired] double WeightKg,
    [property: JsonRequired] ActivityLevel ActivityLevel,
    [property: JsonRequired] GoalType Goal,
    [property: JsonRequired] double DailyCalorieTarget);
