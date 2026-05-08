using FluentAssertions;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;
using Xunit;

namespace NutritionAdvisor.UnitTests.Domain;

public class DomainEntitiesTests
{
    [Fact]
    public void Subscription_Should_UpgradeToPremium()
    {
        var user = new User("test@test.com", "Test", "hash", DateTime.UtcNow, Gender.Male);
        var sub = new Subscription(user);

        sub.Type.Should().Be(SubscriptionType.Free);
        sub.IsActive.Should().BeFalse();

        sub.UpgradeToPremium();

        sub.Type.Should().Be(SubscriptionType.Premium);
        sub.IsActive.Should().BeTrue();
        sub.EndDate.Should().NotBeNull();
    }

    [Fact]
    public void User_Should_UpdateCorrectly()
    {
        var user = new User("test@test.com", "Old", "hash", DateTime.UtcNow, Gender.Male);
        var newDate = new DateTime(1990, 1, 1);
        
        user.Update("New", newDate, Gender.Female);

        user.FullName.Should().Be("New");
        user.Gender.Should().Be(Gender.Female);
        user.DateOfBirth.Year.Should().Be(1990);
    }

    [Fact]
    public void UserProfile_Should_UpdateCorrectly()
    {
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var profile = new UserProfile(user, 180, 80, ActivityLevel.Active, GoalType.Maintain, 2500);

        profile.Update(185, 85, ActivityLevel.VeryActive, GoalType.GainWeight, 3000);

        profile.HeightCm.Should().Be(185);
        profile.WeightKg.Should().Be(85);
        profile.ActivityLevel.Should().Be(ActivityLevel.VeryActive);
        profile.Goal.Should().Be(GoalType.GainWeight);
        profile.DailyCalorieTarget.Should().Be(3000);
    }

    [Fact]
    public void MealPlan_Should_AddEntry()
    {
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var plan = new MealPlan(user, DateTime.UtcNow, DateTime.UtcNow, PlanType.Daily);
        var recipe = new Recipe("Test", "Desc", MealType.Breakfast);
        var entry = new MealPlanEntry(plan, DateTime.UtcNow, MealType.Breakfast, recipe);

        plan.AddEntry(entry);

        plan.Entries.Should().ContainSingle();
    }

    [Fact]
    public void Recipe_Should_AddIngredient()
    {
        var recipe = new Recipe("Test", "Desc", MealType.Breakfast);
        var ingredient = new Ingredient("Apple", 50, 0, 10, 0);
        var recipeIng = new RecipeIngredient(recipe, ingredient, 100);

        recipe.AddIngredient(recipeIng);

        recipe.Ingredients.Should().ContainSingle();
    }
}