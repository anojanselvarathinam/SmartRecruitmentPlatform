using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        builder.HasKey(employer => employer.EmployerId);
        builder.Property(employer => employer.FullName).IsRequired().HasMaxLength(100);
        builder.Property(employer => employer.Email).IsRequired().HasMaxLength(150);
        builder.Property(employer => employer.PasswordHash).IsRequired().HasMaxLength(255);
        builder.HasIndex(employer => employer.UserId).IsUnique();
        builder.HasIndex(employer => employer.Email).IsUnique();

        builder.HasOne(employer => employer.User)
            .WithOne(user => user.Employer)
            .HasForeignKey<Employer>(employer => employer.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
