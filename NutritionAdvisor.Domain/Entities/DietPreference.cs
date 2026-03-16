using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class DietPreference
{
    public Guid Id { get; private set; }
    public DietPreferenceType Name { get; private set; } // Vegan, Keto, etc.
    
    private DietPreference() { }

    public DietPreference(DietPreferenceType name)
    {
        Name = name;
    }
}