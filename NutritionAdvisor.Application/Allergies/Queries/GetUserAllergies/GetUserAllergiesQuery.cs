using MediatR;

namespace NutritionAdvisor.Application.Allergies.Queries.GetUserAllergies;

public record GetUserAllergiesQuery(Guid UserId) : IRequest<List<AllergyDto>>;