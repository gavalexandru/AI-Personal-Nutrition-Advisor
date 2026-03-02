using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class UserAllergyConfiguration : IEntityTypeConfiguration<UserAllergy>
{
    public void Configure(EntityTypeBuilder<UserAllergy> builder)
    {
        builder.Property<Guid>("UserId");
        builder.Property<Guid>("AllergyId");
        builder.HasKey("UserId", "AllergyId");

        builder.HasOne(ua => ua.User).WithMany(u => u.Allergies).HasForeignKey("UserId");
        builder.HasOne(ua => ua.Allergy).WithMany().HasForeignKey("AllergyId");
    }
}