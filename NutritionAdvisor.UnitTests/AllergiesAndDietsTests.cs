using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Allergies.Commands.UpdateUserAllergies;
using NutritionAdvisor.Application.Allergies.Queries.GetUserAllergies;
using NutritionAdvisor.Application.UserDietPreferences.Commands.UpdateUserDietPreferences;
using NutritionAdvisor.Application.UserDietPreferences.Queries.GetUserDietPreferences;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;
using NutritionAdvisor.UnitTests.Common;
using Xunit;

namespace NutritionAdvisor.UnitTests;

public class AllergiesAndDietsTests
{
    [Fact]
    public async Task UpdateAllergies_ShouldSucceed()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var allergy = new Allergy("Peanuts");
        
        context.Users.Add(user);
        context.Allergies.Add(allergy);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateUserAllergiesCommandHandler(context);
        await handler.Handle(new UpdateUserAllergiesCommand(user.Id, new List<Guid> { allergy.Id }), CancellationToken.None);

        var dbUser = await context.Users.Include(u => u.Allergies).FirstAsync();
        dbUser.Allergies.Should().ContainSingle();
    }

    [Fact]
    public async Task GetUserAllergies_ShouldReturnList()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var allergy = new Allergy("Peanuts");
        
        context.Users.Add(user);
        context.Allergies.Add(allergy);
        
        context.UserAllergies.Add(new UserAllergy(user, allergy));
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new GetUserAllergiesQueryHandler(context);
        var res = await handler.Handle(new GetUserAllergiesQuery(user.Id), CancellationToken.None);

        res.Should().ContainSingle(a => a.Name == "Peanuts");
    }

    [Fact]
    public async Task UpdateDietPreferences_ShouldSucceed()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var diet = new DietPreference(DietPreferenceType.Vegan);
        
        context.Users.Add(user);
        context.DietPreferences.Add(diet);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateUserDietPreferencesCommandHandler(context);
        await handler.Handle(new UpdateUserDietPreferencesCommand(user.Id, new List<Guid> { diet.Id }), CancellationToken.None);

        var dbUser = await context.Users.Include(u => u.DietPreferences).FirstAsync();
        dbUser.DietPreferences.Should().ContainSingle();
    }

    [Fact]
    public async Task GetUserDietPreferences_ShouldReturnList()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var diet = new DietPreference(DietPreferenceType.Vegan);
        
        context.Users.Add(user);
        context.DietPreferences.Add(diet);
        
        context.UserDietPreferences.Add(new UserDietPreference(user, diet));
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new GetUserDietPreferencesQueryHandler(context);
        var res = await handler.Handle(new GetUserDietPreferencesQuery(user.Id), CancellationToken.None);

        res.Should().ContainSingle();
    }
}