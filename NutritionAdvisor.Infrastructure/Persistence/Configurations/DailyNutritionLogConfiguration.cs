using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class DailyNutritionLogConfiguration : IEntityTypeConfiguration<DailyNutritionLog>
{
    public void Configure(EntityTypeBuilder<DailyNutritionLog> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}