using MediatR;

namespace NutritionAdvisor.Application.Auth.Commands.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthResult>;