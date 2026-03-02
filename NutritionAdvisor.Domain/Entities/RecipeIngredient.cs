namespace NutritionAdvisor.Domain.Entities;

public class RecipeIngredient
{
    public Recipe Recipe { get; private set; } = null!;
    public Ingredient Ingredient { get; private set; } = null!;

    public double QuantityInGrams { get; private set; }
}