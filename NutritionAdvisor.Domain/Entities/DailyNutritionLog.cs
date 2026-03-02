namespace NutritionAdvisor.Domain.Entities;

public class DailyNutritionLog
{
    public Guid Id { get; private set; }
    public User User { get; private set; } = null!;

    public DateTime Date { get; private set; }

    public double TotalCalories { get; private set; }
    public double TotalProtein { get; private set; }
    public double TotalCarbs { get; private set; }
    public double TotalFat { get; private set; }
}