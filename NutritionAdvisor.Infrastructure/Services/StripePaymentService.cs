using Microsoft.Extensions.Configuration;
using NutritionAdvisor.Application.Common.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NutritionAdvisor.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public StripePaymentService(IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        _successUrl = configuration["Stripe:SuccessUrl"]!;
        _cancelUrl = configuration["Stripe:CancelUrl"]!;
    }

    public async Task<string> CreateSubscriptionCheckoutSessionAsync(Guid userId, string userEmail)
    {
        var options = new SessionCreateOptions
        {
            CustomerEmail = userEmail,
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = "price_1TQTDYQuB1mwcY6thA6ODEQo", 
                    Quantity = 1,
                },
            ],
            Mode = "subscription",
            SuccessUrl = _successUrl,
            CancelUrl = _cancelUrl,
            ClientReferenceId = userId.ToString()
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
}