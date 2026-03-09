using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RegisterCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify if the email already exists
        if (await context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken))
            throw new Exception("Email already exists."); 

        // 2. Hash password
        var hash = passwordHasher.HashPassword(request.Password);

        // 3. Create User
        var user = new User(request.Email, request.FullName, hash, request.DateOfBirth, request.Gender);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        // 4. Generate token
        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthResult(token, user.Email, user.FullName);
    }
}