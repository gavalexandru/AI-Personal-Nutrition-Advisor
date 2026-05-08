using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NutritionAdvisor.Application.Auth.Commands.Register;
using NutritionAdvisor.Domain.Enums;
using Xunit;

namespace NutritionAdvisor.UnitTests.IntegrationTests;

// Aici am schimbat WebApplicationFactory<Program> cu CustomWebApplicationFactory
public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(CustomWebApplicationFactory factory)
    {
        // Pornește API-ul cu baza de date In-Memory și setările din CustomFactory
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnOk_WhenValidRequest()
    {
        // Arrange
        var command = new RegisterCommand(
            Email: $"test_{Guid.NewGuid()}@test.com", 
            Password: "Password123!",
            FullName: "Integration Test User",
            DateOfBirth: DateTime.UtcNow.AddYears(-20),
            Gender: Gender.Male
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("Integration Test User");
    }

    [Fact]
    public async Task LookupEndpoint_ShouldReturnAllergies()
    {
        // Act
        var response = await _client.GetAsync("/api/lookup/allergies");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        
        // În Program.cs avem un script de "Seeding" care adaugă Peanuts automat la start.
        // Cum noi pornim app-ul de la zero In-Memory, seed-ul va rula cu succes!
        content.Should().Contain("Peanuts"); 
    }
}