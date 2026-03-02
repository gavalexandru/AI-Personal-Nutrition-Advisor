using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class AiRecommendationLogConfiguration : IEntityTypeConfiguration<AiRecommendationLog>
{
    public void Configure(EntityTypeBuilder<AiRecommendationLog> builder)
    {
        builder.HasKey(log => log.Id);
        
        builder.Property(log => log.InputSnapshotJson).HasColumnType("jsonb");
        builder.Property(log => log.OutputSnapshotJson).HasColumnType("jsonb");
    }
}