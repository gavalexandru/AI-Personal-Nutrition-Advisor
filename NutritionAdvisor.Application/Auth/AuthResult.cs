namespace NutritionAdvisor.Application.Auth;

public record AuthResult(string Token, string Email, string FullName);