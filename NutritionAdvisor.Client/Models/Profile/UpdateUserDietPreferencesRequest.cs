namespace NutritionAdvisor.Client.Models.Profile;

public class UpdateUserDietPreferencesRequest
{
    public List<Guid> DietPreferenceIds { get; set; } = [];
}