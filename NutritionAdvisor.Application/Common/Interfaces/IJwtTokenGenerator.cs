using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}