using MediatR;

namespace NutritionAdvisor.Application.Allergies.Commands.UpdateUserAllergies;

public record UpdateUserAllergiesCommand(Guid UserId, List<Guid> AllergyIds) : IRequest<bool>;