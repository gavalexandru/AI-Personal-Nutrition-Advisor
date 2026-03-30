using MediatR;

namespace NutritionAdvisor.Application.MealPlans.Queries.GetCurrentMealPlan;

public record GetCurrentMealPlanQuery(Guid UserId) : IRequest<List<MealPlanDayDto>>;