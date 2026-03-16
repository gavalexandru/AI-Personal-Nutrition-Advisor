using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.Allergies.Commands.UpdateUserAllergies;

public class UpdateUserAllergiesCommandHandler(IApplicationDbContext context) 
    : IRequestHandler<UpdateUserAllergiesCommand, bool>
{
    public async Task<bool> Handle(UpdateUserAllergiesCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Allergies)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        var selectedAllergies = await context.Allergies
            .Where(a => request.AllergyIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        user.Allergies.Clear();

        foreach (var allergy in selectedAllergies)
        {
            user.Allergies.Add(new UserAllergy(user, allergy));
        }

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}