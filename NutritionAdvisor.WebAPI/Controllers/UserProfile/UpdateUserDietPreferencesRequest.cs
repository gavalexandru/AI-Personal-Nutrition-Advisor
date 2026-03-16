namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

public class UpdateUserDietPreferencesRequest
{
    public List<Guid> DietPreferenceIds { get; set; } = [];
}