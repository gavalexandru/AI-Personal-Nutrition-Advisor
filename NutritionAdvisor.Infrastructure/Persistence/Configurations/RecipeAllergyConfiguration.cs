using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class RecipeAllergyConfiguration : IEntityTypeConfiguration<RecipeAllergy>
{
    public void Configure(EntityTypeBuilder<RecipeAllergy> builder)
    {
        builder.Property<Guid>("RecipeId");
        builder.Property<Guid>("AllergyId");
        builder.HasKey("RecipeId", "AllergyId");

        builder.HasOne(ra => ra.Recipe).WithMany(r => r.Allergies).HasForeignKey("RecipeId");
        builder.HasOne(ra => ra.Allergy).WithMany().HasForeignKey("AllergyId");
    }
}