using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NutritionAdvisor.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController(IApplicationDbContext context, IConfiguration config) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var endpointSecret = config["Stripe:WebhookSecret"]; 

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                if (stripeEvent.Data.Object is Session session && Guid.TryParse(session.ClientReferenceId, out var userId))
                {
                    var user = await context.Users
                        .Include(u => u.Subscription)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (user != null)
                    {
                        user.Subscription.UpgradeToPremium();
                        await context.SaveChangesAsync(CancellationToken.None);
                    }
                }
            }
            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }
}