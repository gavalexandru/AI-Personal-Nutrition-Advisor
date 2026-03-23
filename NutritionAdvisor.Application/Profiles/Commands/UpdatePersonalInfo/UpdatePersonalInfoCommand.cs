using MediatR;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.Commands.UpdatePersonalInfo;

public record UpdatePersonalInfoCommand(
    Guid UserId, 
    string FullName, 
    Gender Gender, 
    DateTime DateOfBirth
) : IRequest<bool>;