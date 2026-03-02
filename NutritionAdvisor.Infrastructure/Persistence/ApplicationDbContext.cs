using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<UserAllergy> UserAllergies => Set<UserAllergy>();
    public DbSet<DietPreference> DietPreferences => Set<DietPreference>();
    public DbSet<UserDietPreference> UserDietPreferences => Set<UserDietPreference>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<RecipeAllergy> RecipeAllergies => Set<RecipeAllergy>();
    public DbSet<RecipeDietPreference> RecipeDietPreferences => Set<RecipeDietPreference>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();
    public DbSet<MealPlanEntry> MealPlanEntries => Set<MealPlanEntry>();
    public DbSet<AiRecommendationLog> AiRecommendationLogs => Set<AiRecommendationLog>();
    public DbSet<WeightLog> WeightLogs => Set<WeightLog>();
    public DbSet<DailyNutritionLog> DailyNutritionLogs => Set<DailyNutritionLog>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}