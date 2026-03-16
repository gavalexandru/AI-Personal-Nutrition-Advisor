using MediatR;

namespace NutritionAdvisor.Application.Profiles.Queries.GetProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto?>;