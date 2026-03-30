using NutritionAdvisor.Application.MealPlans.Models;

namespace NutritionAdvisor.Application.Common.Interfaces;

public interface IAiRecommendationService
{
    Task<AiMealPlanResponse> GenerateMealPlanAsync(AiMealPlanRequest request, CancellationToken cancellationToken = default);
}