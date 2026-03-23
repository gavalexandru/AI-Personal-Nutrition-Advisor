using MediatR;
using NutritionAdvisor.Application.Profiles.DTOs;

namespace NutritionAdvisor.Application.Profiles.Queries.GetPersonalInfo;

public record GetPersonalInfoQuery(Guid UserId) : IRequest<PersonalInfoDto>;