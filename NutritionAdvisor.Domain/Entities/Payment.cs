using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;

    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
}