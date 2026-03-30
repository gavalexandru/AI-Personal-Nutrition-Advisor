using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class Recipe(string name, string description, MealType mealType)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public MealType MealType { get; private set; } = mealType;

    public ICollection<RecipeIngredient> Ingredients { get; private set; } = new List<RecipeIngredient>();
	
    public ICollection<RecipeAllergy> Allergies { get; private set; } = new List<RecipeAllergy>();
    public ICollection<RecipeDietPreference> DietPreferences { get; private set; } = new List<RecipeDietPreference>();

    public void AddIngredient(RecipeIngredient ingredient)
    {
        Ingredients.Add(ingredient);
    }
}