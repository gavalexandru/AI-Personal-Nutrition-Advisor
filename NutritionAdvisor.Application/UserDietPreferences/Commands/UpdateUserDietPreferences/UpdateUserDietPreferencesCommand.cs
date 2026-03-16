using MediatR;

namespace NutritionAdvisor.Application.UserDietPreferences.Commands.UpdateUserDietPreferences;

public record UpdateUserDietPreferencesCommand(Guid UserId, List<Guid> DietPreferenceIds) : IRequest<bool>;