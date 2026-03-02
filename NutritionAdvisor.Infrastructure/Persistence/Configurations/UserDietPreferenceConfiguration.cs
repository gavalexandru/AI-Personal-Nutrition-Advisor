using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionAdvisor.Domain.Entities;

namespace NutritionAdvisor.Infrastructure.Persistence.Configurations;

public class UserDietPreferenceConfiguration : IEntityTypeConfiguration<UserDietPreference>
{
    public void Configure(EntityTypeBuilder<UserDietPreference> builder)
    {
        builder.Property<Guid>("UserId");
        builder.Property<Guid>("DietPreferenceId");
        builder.HasKey("UserId", "DietPreferenceId");

        builder.HasOne(udp => udp.User).WithMany(u => u.DietPreferences).HasForeignKey("UserId");
        builder.HasOne(udp => udp.DietPreference).WithMany().HasForeignKey("DietPreferenceId");
    }
}