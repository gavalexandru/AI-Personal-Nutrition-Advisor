using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string HashedPassword { get; private set; } = null!;
    public RoleType Role { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public Subscription Subscription { get; private set; } = null!;

    public UserProfile Profile { get; private set; } = null!;
    public ICollection<UserAllergy> Allergies { get; private set; } = null!;
    public ICollection<UserDietPreference> DietPreferences { get; private set; } = null!;
    public ICollection<MealPlan> MealPlans { get; private set; } = null!;
    
    private User() { } 

    public User(string email, string fullName, string hashedPassword, DateTime dateOfBirth, Gender gender)
    {
        Id = Guid.NewGuid();
        Email = email;
        FullName = fullName;
        HashedPassword = hashedPassword;
        DateOfBirth = dateOfBirth.ToUniversalTime();
        Gender = gender;
        Role = RoleType.User;
    }
    
}