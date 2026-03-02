namespace NutritionAdvisor.Domain.Entities;

public class RecipeDietPreference
{
    public Recipe Recipe { get; private set; } = null!;
    public DietPreference DietPreference { get; private set; } = null!;
}