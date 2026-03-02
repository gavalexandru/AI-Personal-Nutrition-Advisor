using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public MealType MealType { get; private set; }

    public ICollection<RecipeIngredient> Ingredients { get; private set; } = null!;
	
    public ICollection<RecipeAllergy> Allergies { get; private set; } = null!;
    public ICollection<RecipeDietPreference> DietPreferences { get; private set; } = null!;
}