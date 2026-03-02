namespace NutritionAdvisor.Domain.Entities;

public class WeightLog
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public DateTime Date { get; private set; }
    public double WeightKg { get; private set; }
}