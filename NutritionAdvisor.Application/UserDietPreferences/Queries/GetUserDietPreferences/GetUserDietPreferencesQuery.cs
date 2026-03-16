using MediatR;

namespace NutritionAdvisor.Application.UserDietPreferences.Queries.GetUserDietPreferences;

public record GetUserDietPreferencesQuery(Guid UserId) : IRequest<List<DietPreferenceDto>>;