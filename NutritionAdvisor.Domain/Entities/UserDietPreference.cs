namespace NutritionAdvisor.Domain.Entities;

public class UserDietPreference
{
    public User User { get; private set; } = null!;
    public DietPreference DietPreference { get; private set; } = null!;
    
    private UserDietPreference() { }
    
    public UserDietPreference(User user, DietPreference dietPreference)
    {
        User = user;
        DietPreference = dietPreference;
    }
}