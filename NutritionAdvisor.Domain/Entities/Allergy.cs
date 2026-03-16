namespace NutritionAdvisor.Domain.Entities;

public class Allergy
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    private Allergy() { }

    public Allergy(string name)
    {
        Name = name;
    }
}