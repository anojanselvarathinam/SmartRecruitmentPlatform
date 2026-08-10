using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(company => company.CompanyId);
        builder.Property(company => company.CompanyName).IsRequired().HasMaxLength(150);
        builder.Property(company => company.Description).HasMaxLength(2000);
        builder.Property(company => company.Location).IsRequired().HasMaxLength(150);
        builder.Property(company => company.Industry).IsRequired().HasMaxLength(100);
        builder.Property(company => company.Website).HasMaxLength(300);
        builder.HasIndex(company => company.EmployerId).IsUnique();

        builder.HasOne(company => company.Employer)
            .WithOne(employer => employer.Company)
            .HasForeignKey<Company>(company => company.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
