using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.Application.Profiles.DTOs;

public class UpdatePersonalInfoRequest
{
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
}