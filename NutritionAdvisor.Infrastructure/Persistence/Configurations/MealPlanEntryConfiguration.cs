using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class MealPlanEntryConfiguration : IEntityTypeConfiguration<MealPlanEntry>
{
    public void Configure(EntityTypeBuilder<MealPlanEntry> builder)
    {
        builder.HasKey(mpe => mpe.Id);
        builder.Property(mpe => mpe.MealType).HasConversion<string>();

        builder.HasOne(mpe => mpe.MealPlan)
            .WithMany(mp => mp.Entries)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mpe => mpe.Recipe)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict); 
    }
}