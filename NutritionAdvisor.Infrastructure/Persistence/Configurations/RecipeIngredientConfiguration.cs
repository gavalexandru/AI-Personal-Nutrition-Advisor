using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.Property<Guid>("RecipeId");
        builder.Property<Guid>("IngredientId");
        builder.HasKey("RecipeId", "IngredientId");

        builder.HasOne(ri => ri.Recipe).WithMany(r => r.Ingredients).HasForeignKey("RecipeId");
        builder.HasOne(ri => ri.Ingredient).WithMany().HasForeignKey("IngredientId");
    }
}