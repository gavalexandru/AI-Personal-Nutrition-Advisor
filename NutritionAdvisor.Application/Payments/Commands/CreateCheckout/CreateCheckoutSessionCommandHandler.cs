using MediatR;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.Application.Payments.Commands.CreateCheckout;

public class CreateCheckoutSessionCommandHandler(IPaymentService paymentService) 
    : IRequestHandler<CreateCheckoutSessionCommand, string>
{
    public async Task<string> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        return await paymentService.CreateSubscriptionCheckoutSessionAsync(request.UserId, request.Email);
    }
}