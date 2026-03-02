namespace NutritionAdvisor.Domain.Entities;

public class Allergy
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
}