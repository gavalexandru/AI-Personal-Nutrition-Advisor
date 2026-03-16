using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.Profiles.Commands.CreateProfile;

public class CreateUserProfileCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateUserProfileCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new Exception("User not found.");

        if (user.Profile != null)
            throw new Exception("User already has a profile.");

       
        var profile = new UserProfile(
            user,
            request.HeightCm,
            request.WeightKg,
            request.ActivityLevel,
            request.Goal,
            request.DailyCalorieTarget);

        context.UserProfiles.Add(profile);
        await context.SaveChangesAsync(cancellationToken);

        return profile.Id;
    }
}