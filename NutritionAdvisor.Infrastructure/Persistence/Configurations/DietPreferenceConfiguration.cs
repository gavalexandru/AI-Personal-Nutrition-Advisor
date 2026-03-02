using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class DietPreferenceConfiguration : IEntityTypeConfiguration<DietPreference>
{
    public void Configure(EntityTypeBuilder<DietPreference> builder)
    {
        builder.HasKey(dp => dp.Id);
        builder.Property(dp => dp.Name).HasConversion<string>(); 
    }
}