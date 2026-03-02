namespace NutritionAdvisor.Domain.Entities;

public class RecipeAllergy
{
    public Recipe Recipe { get; private set; } = null!;
    public Allergy Allergy { get; private set; } = null!;
}