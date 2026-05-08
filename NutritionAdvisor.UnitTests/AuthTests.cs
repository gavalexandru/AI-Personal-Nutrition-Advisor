using FluentAssertions;
using Moq;
using NutritionAdvisor.Application.Auth.Commands.Queries.Login;
using NutritionAdvisor.Application.Auth.Commands.Register;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;
using NutritionAdvisor.UnitTests.Common;
using Xunit;

namespace NutritionAdvisor.UnitTests;

public class AuthTests
{
    [Fact]
    public async Task Register_ShouldCreateUser_WhenValid()
    {
        await using var context = TestDbContextFactory.Create();
        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed");
        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("token");

        var handler = new RegisterCommandHandler(context, hasher.Object, jwt.Object);
        var result = await handler.Handle(new RegisterCommand("t@t.com", "pass", "Test", DateTime.UtcNow, Gender.Male), CancellationToken.None);

        result.Token.Should().Be("token");
        context.Users.Should().ContainSingle();
    }

    [Fact]
    public async Task Register_ShouldThrowException_WhenEmailExists()
    {
        await using var context = TestDbContextFactory.Create();
        context.Users.Add(new User("t@t.com", "Test", "hash", DateTime.UtcNow, Gender.Male));
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new RegisterCommandHandler(context, new Mock<IPasswordHasher>().Object, new Mock<IJwtTokenGenerator>().Object);
        
        Func<Task> act = async () => await handler.Handle(new RegisterCommand("t@t.com", "pass", "Test", DateTime.UtcNow, Gender.Male), CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().WithMessage("Email already exists.");
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenValid()
    {
        await using var context = TestDbContextFactory.Create();
        var user = new User("t@t.com", "Test", "hash", DateTime.UtcNow, Gender.Male);
        context.Users.Add(user);
        await context.SaveChangesAsync(CancellationToken.None);

        var hasher = new Mock<IPasswordHasher>();
        hasher.Setup(x => x.VerifyPassword("pass", "hash")).Returns(true);
        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("token");

        var handler = new LoginQueryHandler(context, hasher.Object, jwt.Object);
        var result = await handler.Handle(new LoginQuery("t@t.com", "pass"), CancellationToken.None);

        result.Token.Should().Be("token");
    }

    [Fact]
    public async Task Login_ShouldThrowException_WhenInvalidCredentials()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new LoginQueryHandler(context, new Mock<IPasswordHasher>().Object, new Mock<IJwtTokenGenerator>().Object);
        
        Func<Task> act = async () => await handler.Handle(new LoginQuery("wrong@test.com", "pass"), CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().WithMessage("Invalid credentials.");
    }
}