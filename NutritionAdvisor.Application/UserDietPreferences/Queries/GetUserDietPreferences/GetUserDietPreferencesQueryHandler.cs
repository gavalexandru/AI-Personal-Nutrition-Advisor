using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.UserDietPreferences.Queries.GetUserDietPreferences;

public class GetUserDietPreferencesQueryHandler(IApplicationDbContext context) 
    : IRequestHandler<GetUserDietPreferencesQuery, List<DietPreferenceDto>>
{
    public async Task<List<DietPreferenceDto>> Handle(GetUserDietPreferencesQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.DietPreferences)
            .ThenInclude(udp => udp.DietPreference)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) throw new Exception("User not found.");

        return user.DietPreferences
            .Select(udp => new DietPreferenceDto(
                udp.DietPreference.Id, 
                udp.DietPreference.Name.ToString())) 
            .ToList();
    }
}