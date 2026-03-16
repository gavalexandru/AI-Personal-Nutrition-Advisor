namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

public class UpdateUserAllergiesRequest
{
    public List<Guid> AllergyIds { get; set; } = [];
}