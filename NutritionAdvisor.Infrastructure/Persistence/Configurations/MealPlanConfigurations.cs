using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class MealPlanConfiguration : IEntityTypeConfiguration<MealPlan>
{
    public void Configure(EntityTypeBuilder<MealPlan> builder)
    {
        builder.HasKey(mp => mp.Id);
        builder.Property(mp => mp.PlanType).HasConversion<string>();

        builder.HasOne(mp => mp.User)
            .WithMany(u => u.MealPlans)
            .OnDelete(DeleteBehavior.Cascade);
    }
}