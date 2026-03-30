using System.Net.Http.Json;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Application.MealPlans.Models;

namespace NutritionAdvisor.Infrastructure.Services;

public class PythonAiRecommendationService(HttpClient httpClient) : IAiRecommendationService
{
    public async Task<AiMealPlanResponse> GenerateMealPlanAsync(AiMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        
        var response = await httpClient.PostAsJsonAsync("http://127.0.0.1:8000/api/ai/generate-plan", request, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AiMealPlanResponse>(cancellationToken: cancellationToken);
        
        return result ?? new AiMealPlanResponse();
    }
}