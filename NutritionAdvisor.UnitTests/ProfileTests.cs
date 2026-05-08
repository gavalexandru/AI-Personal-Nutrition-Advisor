using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Profiles.Commands.CreateProfile;
using NutritionAdvisor.Application.Profiles.Commands.UpdatePersonalInfo;
using NutritionAdvisor.Application.Profiles.Commands.UpdateProfile;
using NutritionAdvisor.Application.Profiles.Queries.GetPersonalInfo;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;
using NutritionAdvisor.UnitTests.Common;
using Xunit;

namespace NutritionAdvisor.UnitTests;

public class ProfileTests
{
    [Fact]
    public async Task CreateProfile_ShouldSucceed()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        context.Users.Add(user);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new CreateUserProfileCommandHandler(context);
        var result = await handler.Handle(new CreateUserProfileCommand(user.Id, 180, 80, ActivityLevel.Active, GoalType.Maintain, 2500), CancellationToken.None);

        result.Should().NotBeEmpty();
        context.UserProfiles.Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateProfile_ShouldSucceed()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        var profile = new UserProfile(user, 180, 80, ActivityLevel.Active, GoalType.Maintain, 2500);
        context.Users.Add(user);
        context.UserProfiles.Add(profile);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateUserProfileCommandHandler(context);
        var result = await handler.Handle(new UpdateUserProfileCommand(user.Id, 190, 90, ActivityLevel.Sedentary, GoalType.LoseWeight, 2000), CancellationToken.None);

        result.Should().BeTrue();
        var updated = await context.UserProfiles.FirstAsync();
        updated.HeightCm.Should().Be(190);
    }

    [Fact]
    public async Task GetPersonalInfo_ShouldReturnDto()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        context.Users.Add(user);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new GetPersonalInfoQueryHandler(context);
        var result = await handler.Handle(new GetPersonalInfoQuery(user.Id), CancellationToken.None);

        result.Email.Should().Be("t@t.com");
    }

    [Fact]
    public async Task UpdatePersonalInfo_ShouldUpdateUser()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "T", "h", DateTime.UtcNow, Gender.Male);
        context.Users.Add(user);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdatePersonalInfoCommandHandler(context);
        await handler.Handle(new UpdatePersonalInfoCommand(user.Id, "NewName", Gender.Female, DateTime.UtcNow), CancellationToken.None);

        var updated = await context.Users.FirstAsync();
        updated.FullName.Should().Be("NewName");
        updated.Gender.Should().Be(Gender.Female);
    }
}