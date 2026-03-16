using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Allergies.Queries.GetUserAllergies;

public class GetUserAllergiesQueryHandler(IApplicationDbContext context) 
    : IRequestHandler<GetUserAllergiesQuery, List<AllergyDto>>
{
    public async Task<List<AllergyDto>> Handle(GetUserAllergiesQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Allergies)
            .ThenInclude(ua => ua.Allergy) 
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) throw new Exception("User not found.");

        return user.Allergies
            .Select(ua => new AllergyDto(ua.Allergy.Id, ua.Allergy.Name))
            .ToList();
    }
}