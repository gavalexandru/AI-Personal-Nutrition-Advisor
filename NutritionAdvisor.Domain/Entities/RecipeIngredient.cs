namespace NutritionAdvisor.Domain.Entities;

public class RecipeIngredient(Recipe recipe, Ingredient ingredient, double quantityInGrams)
{
    private RecipeIngredient() : this(null!, null!, 0) { }
    public Recipe Recipe { get; private set; } = recipe;
    public Ingredient Ingredient { get; private set; } = ingredient;

    public double QuantityInGrams { get; private set; } = quantityInGrams;
}