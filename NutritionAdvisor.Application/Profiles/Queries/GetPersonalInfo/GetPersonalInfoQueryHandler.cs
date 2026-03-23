using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Application.Profiles.DTOs;

namespace NutritionAdvisor.Application.Profiles.Queries.GetPersonalInfo;

public class GetPersonalInfoQueryHandler(IApplicationDbContext context) 
    : IRequestHandler<GetPersonalInfoQuery, PersonalInfoDto>
{
    public async Task<PersonalInfoDto> Handle(GetPersonalInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Subscription)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new Exception("User not found.");

        return new PersonalInfoDto
        {
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            Subscription = user.Subscription.Type
        };
    }
}