using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.MealPlans.Queries.GetCurrentMealPlan;

public class GetCurrentMealPlanQueryHandler(IApplicationDbContext context) 
    : IRequestHandler<GetCurrentMealPlanQuery, List<MealPlanDayDto>>
{
    public async Task<List<MealPlanDayDto>> Handle(GetCurrentMealPlanQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        
        var mealPlan = await context.MealPlans
            .Include(mp => mp.Entries)
            .ThenInclude(e => e.Recipe)
            .ThenInclude(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .Where(mp => mp.User.Id == request.UserId && mp.EndDate >= today)
            .OrderByDescending(mp => mp.StartDate) 
            .FirstOrDefaultAsync(cancellationToken);

        if (mealPlan == null)
            return [];
        
        var groupedDays = mealPlan.Entries
            .GroupBy(e => (e.Date.Date - mealPlan.StartDate.Date).Days + 1)
            .Select(g => new MealPlanDayDto
            {
                Day = g.Key,
                Meals = g.Select(e => 
                {
                    var mainIngredient = e.Recipe.Ingredients.FirstOrDefault()?.Ingredient;
                    return new MealDto
                    {
                        MealType = e.MealType.ToString(),
                        Name = e.Recipe.Name,
                        Description = e.Recipe.Description,
                        Calories = mainIngredient != null ? Math.Round(mainIngredient.CaloriesPer100G) : 0,
                        Protein = mainIngredient != null ? Math.Round(mainIngredient.ProteinPer100G) : 0,
                        Fat = mainIngredient != null ? Math.Round(mainIngredient.FatPer100G) : 0,
                        Carbs = mainIngredient != null ? Math.Round(mainIngredient.CarbsPer100G) : 0
                    };
                }).ToList()
            })
            .OrderBy(d => d.Day)
            .ToList();

        return groupedDays;
    }
}