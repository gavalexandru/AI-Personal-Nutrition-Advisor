using NutritionAdvisor.Domain.Entities;
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.DTOs;

public class PersonalInfoDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RoleType Role { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public SubscriptionType Subscription { get; set; }
}