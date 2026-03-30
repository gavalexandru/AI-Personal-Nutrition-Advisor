using MediatR;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.MealPlans.Commands.GenerateMealPlan;

public record GenerateMealPlanCommand(Guid UserId, PlanType PlanType) : IRequest<Guid>;