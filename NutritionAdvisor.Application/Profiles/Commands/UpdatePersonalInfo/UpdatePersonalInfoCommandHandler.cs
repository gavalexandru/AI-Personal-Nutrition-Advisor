using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Profiles.Commands.UpdatePersonalInfo;

public class UpdatePersonalInfoCommandHandler(IApplicationDbContext context) 
    : IRequestHandler<UpdatePersonalInfoCommand, bool>
{
    public async Task<bool> Handle(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new Exception("User not found.");

        user.Update(request.FullName, request.DateOfBirth, request.Gender);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}