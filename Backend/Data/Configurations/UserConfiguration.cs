using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.Authentication;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.UserId);
        builder.Property(user => user.FullName).IsRequired().HasMaxLength(100);
        builder.Property(user => user.Email).IsRequired().HasMaxLength(150);
        builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(user => user.Role).IsRequired().HasMaxLength(30);
        builder.HasIndex(user => user.Email).IsUnique();
    }
}
