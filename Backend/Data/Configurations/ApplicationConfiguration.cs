using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(application => application.ApplicationId);
        builder.Property(application => application.MatchScore).HasPrecision(5, 2);
        builder.Property(application => application.Status).IsRequired().HasMaxLength(30);

        builder.HasIndex(application => new
        {
            application.JobId,
            application.JobSeekerId
        }).IsUnique();

        builder.HasOne(application => application.Job)
            .WithMany(job => job.Applications)
            .HasForeignKey(application => application.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(application => application.JobSeeker)
            .WithMany(profile => profile.Applications)
            .HasForeignKey(application => application.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
