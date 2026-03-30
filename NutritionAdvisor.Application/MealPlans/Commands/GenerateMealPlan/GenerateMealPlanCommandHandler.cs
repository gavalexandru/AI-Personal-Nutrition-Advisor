using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Application.MealPlans.Models;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.MealPlans.Commands.GenerateMealPlan;

public class GenerateMealPlanCommandHandler(
    IApplicationDbContext context, 
    IAiRecommendationService aiService) 
    : IRequestHandler<GenerateMealPlanCommand, Guid>
{
    public async Task<Guid> Handle(GenerateMealPlanCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Profile)
            .Include(u => u.Allergies).ThenInclude(a => a.Allergy)
            .Include(u => u.DietPreferences).ThenInclude(dp => dp.DietPreference)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken) 
            ?? throw new Exception("User not found.");

        var existingPlans = await context.MealPlans
            .Where(mp => mp.User.Id == request.UserId && mp.EndDate >= DateTime.UtcNow.Date)
            .ToListAsync(cancellationToken);
            
        context.MealPlans.RemoveRange(existingPlans);

        int days = request.PlanType == PlanType.Daily ? 1 : 7;

        var aiRequest = new AiMealPlanRequest
        {
            DailyCalorieTarget = user.Profile.DailyCalorieTarget,
            Allergies = user.Allergies.Select(a => a.Allergy.Name).ToList(),
            DietPreferences = user.DietPreferences.Select(dp => dp.DietPreference.Name.ToString()).ToList(),
            Days = days,
            
            Goal = user.Profile.Goal.ToString(),
            ActivityLevel = user.Profile.ActivityLevel.ToString()
        };

        var aiResponse = await aiService.GenerateMealPlanAsync(aiRequest, cancellationToken);

        var startDate = DateTime.UtcNow.Date;
        
        var mealPlan = new MealPlan(user, startDate, startDate.AddDays(days - 1), request.PlanType);

        foreach (var dayPlan in aiResponse.PlanDays)
        {
            var entryDate = startDate.AddDays(dayPlan.Day - 1);

            foreach (var aiMeal in dayPlan.Meals)
            {
                var mealTypeEnum = Enum.Parse<MealType>(aiMeal.MealType);

                var ingredient = new Ingredient(
                    $"{aiMeal.Name} Base", 
                    aiMeal.Calories, 
                    aiMeal.Protein, 
                    aiMeal.Carbs, 
                    aiMeal.Fat);
                    
                context.Ingredients.Add(ingredient);

                var recipe = new Recipe(aiMeal.Name, aiMeal.Description, mealTypeEnum);
                var recipeIngredient = new RecipeIngredient(recipe, ingredient, 100);
                
                recipe.AddIngredient(recipeIngredient);
                context.Recipes.Add(recipe);

                var entry = new MealPlanEntry(mealPlan, entryDate, mealTypeEnum, recipe);
                mealPlan.AddEntry(entry);
            }
        }

        context.MealPlans.Add(mealPlan);
        await context.SaveChangesAsync(cancellationToken);

        return mealPlan.Id;
    }
}