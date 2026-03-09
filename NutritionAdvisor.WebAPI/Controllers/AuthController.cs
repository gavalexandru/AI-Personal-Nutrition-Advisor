using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionAdvisor.Application.Auth.Commands.Queries.Login;
using NutritionAdvisor.Application.Auth.Commands.Register;

namespace NutritionAdvisor.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.None, 
            Expires = DateTime.UtcNow.AddHours(2)
        };
        Response.Cookies.Append("jwtToken", token, cookieOptions);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            SetTokenCookie(result.Token);
            return Ok(new { result.Email, result.FullName });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        try
        {
            var result = await mediator.Send(query);
            SetTokenCookie(result.Token);
            return Ok(new { result.Email, result.FullName });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message); 
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var fullName = User.FindFirstValue("FullName");
        return Ok(new { Email = email, FullName = fullName });
    }
}