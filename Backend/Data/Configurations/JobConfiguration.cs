using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(job => job.JobId);
        builder.Property(job => job.JobTitle).IsRequired().HasMaxLength(150);
        builder.Property(job => job.Description).IsRequired().HasMaxLength(4000);
        builder.Property(job => job.RequiredSkills).IsRequired().HasMaxLength(2000);
        builder.Property(job => job.Education).IsRequired().HasMaxLength(150);
        builder.Property(job => job.Location).IsRequired().HasMaxLength(150);

        builder.HasOne(job => job.Company)
            .WithMany(company => company.Jobs)
            .HasForeignKey(job => job.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
