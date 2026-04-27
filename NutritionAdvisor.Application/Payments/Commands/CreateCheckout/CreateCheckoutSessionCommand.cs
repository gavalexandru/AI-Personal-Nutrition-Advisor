using MediatR;

namespace NutritionAdvisor.Application.Payments.Commands.CreateCheckout;

public record CreateCheckoutSessionCommand(Guid UserId, string Email) : IRequest<string>;