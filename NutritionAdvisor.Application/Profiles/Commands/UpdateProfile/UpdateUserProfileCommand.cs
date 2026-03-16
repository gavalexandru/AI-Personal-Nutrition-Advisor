using MediatR;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.Commands.UpdateProfile;

public record UpdateUserProfileCommand(
    Guid UserId,
    double HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    GoalType Goal,
    double DailyCalorieTarget) : IRequest<bool>;