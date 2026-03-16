using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Profiles.Queries.GetProfile;

public class GetUserProfileQueryHandler(IApplicationDbContext context) 
    : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
{
    public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await context.UserProfiles
            .AsNoTracking() 
            .FirstOrDefaultAsync(p => p.User.Id == request.UserId, cancellationToken);

        if (profile == null) return null;

        return new UserProfileDto(
            profile.HeightCm,
            profile.WeightKg,
            profile.ActivityLevel,
            profile.Goal,
            profile.DailyCalorieTarget);
    }
}