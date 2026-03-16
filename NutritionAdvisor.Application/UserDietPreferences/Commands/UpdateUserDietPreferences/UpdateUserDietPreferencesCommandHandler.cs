using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.UserDietPreferences.Commands.UpdateUserDietPreferences;

public class UpdateUserDietPreferencesCommandHandler(IApplicationDbContext context) 
    : IRequestHandler<UpdateUserDietPreferencesCommand, bool>
{
    public async Task<bool> Handle(UpdateUserDietPreferencesCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.DietPreferences)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) throw new Exception("User not found.");

        var selectedPreferences = await context.DietPreferences
            .Where(dp => request.DietPreferenceIds.Contains(dp.Id))
            .ToListAsync(cancellationToken);

        user.DietPreferences.Clear();

        foreach (var preference in selectedPreferences)
        {
            user.DietPreferences.Add(new UserDietPreference(user, preference));
        }

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}