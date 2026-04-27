using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionAdvisor.Application.Payments.Commands.CreateCheckout;

namespace NutritionAdvisor.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-checkout")]
    public async Task<IActionResult> CreateCheckout()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (!Guid.TryParse(userIdString, out var userId) || string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized();
        }

        var command = new CreateCheckoutSessionCommand(userId, userEmail);
        var checkoutUrl = await mediator.Send(command);

        return Ok(new { Url = checkoutUrl });
    }
}