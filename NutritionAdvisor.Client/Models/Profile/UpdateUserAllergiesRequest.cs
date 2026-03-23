namespace NutritionAdvisor.Client.Models.Profile;

public class UpdateUserAllergiesRequest
{
    public List<Guid> AllergyIds { get; set; } = [];
}