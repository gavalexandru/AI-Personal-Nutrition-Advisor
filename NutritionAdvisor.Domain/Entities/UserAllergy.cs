namespace NutritionAdvisor.Domain.Entities;

public class UserAllergy
{
    public User User { get; private set; } = null!;
    public Allergy Allergy { get; private set; } = null!;
    
    private UserAllergy() { }
    
    public UserAllergy(User user, Allergy allergy)
    {
        User = user;
        Allergy = allergy;
    }
}