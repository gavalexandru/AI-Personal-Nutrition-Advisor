using MediatR;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.Commands.CreateProfile;

public record CreateUserProfileCommand(
    Guid UserId,
    double HeightCm,
    double WeightKg,
    ActivityLevel ActivityLevel,
    GoalType Goal,
    double DailyCalorieTarget) : IRequest<Guid>;