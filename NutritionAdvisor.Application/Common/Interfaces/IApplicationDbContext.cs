using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<Allergy> Allergies { get; }
    DbSet<DietPreference> DietPreferences { get; }
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<MealPlan> MealPlans { get; }
    DbSet<MealPlanEntry> MealPlanEntries { get; }
    DbSet<AiRecommendationLog> AiRecommendationLogs { get; }
    DbSet<WeightLog> WeightLogs { get; }
    DbSet<DailyNutritionLog> DailyNutritionLogs { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<Payment> Payments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}