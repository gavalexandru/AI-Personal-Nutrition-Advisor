namespace NutritionAdvisor.Domain.Entities;

public class Ingredient
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public double CaloriesPer100G { get; private set; }
    public double ProteinPer100G { get; private set; } 
    public double CarbsPer100G { get; private set; }
    public double FatPer100G { get; private set; }
}