using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class WeightLogConfiguration : IEntityTypeConfiguration<WeightLog>
{
    public void Configure(EntityTypeBuilder<WeightLog> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}