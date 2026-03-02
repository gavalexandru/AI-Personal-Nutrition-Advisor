using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Status).HasConversion<string>();
        
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}