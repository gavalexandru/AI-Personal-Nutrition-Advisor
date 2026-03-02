using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class RecipeDietPreferenceConfiguration : IEntityTypeConfiguration<RecipeDietPreference>
{
    public void Configure(EntityTypeBuilder<RecipeDietPreference> builder)
    {
        builder.Property<Guid>("RecipeId");
        builder.Property<Guid>("DietPreferenceId");
        builder.HasKey("RecipeId", "DietPreferenceId");

        builder.HasOne(rdp => rdp.Recipe).WithMany(r => r.DietPreferences).HasForeignKey("RecipeId");
        builder.HasOne(rdp => rdp.DietPreference).WithMany().HasForeignKey("DietPreferenceId");
    }
}