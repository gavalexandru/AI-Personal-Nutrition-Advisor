using MediatR;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Email, 
    string Password, 
    string FullName, 
    DateTime DateOfBirth, 
    Gender Gender) : IRequest<AuthResult>;