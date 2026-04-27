namespace NutritionAdvisor.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<string> CreateSubscriptionCheckoutSessionAsync(Guid userId, string userEmail);
}