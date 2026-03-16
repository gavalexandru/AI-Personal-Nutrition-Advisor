using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Profiles.Commands.UpdateProfile;

public class UpdateUserProfileCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateUserProfileCommand, bool>
{
    public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(p => p.User.Id == request.UserId, cancellationToken);

        if (profile == null)
            throw new Exception("Profile not found.");

        profile.Update(
            request.HeightCm,
            request.WeightKg,
            request.ActivityLevel,
            request.Goal,
            request.DailyCalorieTarget);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}