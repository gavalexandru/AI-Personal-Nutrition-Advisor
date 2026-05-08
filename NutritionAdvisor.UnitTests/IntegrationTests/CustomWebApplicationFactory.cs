using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Infrastructure.Persistence;
using Moq;

namespace NutritionAdvisor.UnitTests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("JWT_SECRET", "ThisIsAVerySecretKeyForTestingPurposesOnly12345!");

        builder.ConfigureServices(services =>
        {
            // 1. Ștergem opțiunile vechi
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // 2. Creăm un ServiceProvider complet NOU și IZOLAT doar pentru In-Memory Database.
            // Asta previne conflictul cu orice a lăsat PostgreSQL (Npgsql) în urmă.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // 3. Adăugăm ApplicationDbContext folosind provider-ul izolat
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDb");
                options.UseInternalServiceProvider(serviceProvider); 
            });

            // 4. Mock pentru serviciul AI
            services.RemoveAll(typeof(IAiRecommendationService));
            var mockAiService = new Mock<IAiRecommendationService>();
            services.AddScoped<IAiRecommendationService>(_ => mockAiService.Object);
        });
    }
}