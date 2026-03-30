namespace NutritionAdvisor.Domain.Entities;

public class Ingredient(string name, double caloriesPer100G, double proteinPer100G, double carbsPer100G, double fatPer100G)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;

    public double CaloriesPer100G { get; private set; } = caloriesPer100G;
    public double ProteinPer100G { get; private set; } = proteinPer100G;
    public double CarbsPer100G { get; private set; } = carbsPer100G;
    public double FatPer100G { get; private set; } = fatPer100G;
}