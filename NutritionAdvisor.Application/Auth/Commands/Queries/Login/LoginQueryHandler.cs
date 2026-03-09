using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Auth.Commands.Queries.Login;

public class LoginQueryHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginQuery, AuthResult>
{
    public async Task<AuthResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);
        
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.HashedPassword))
            throw new Exception("Invalid credentials."); 

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthResult(token, user.Email, user.FullName);
    }
}