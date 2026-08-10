using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class JobSeekerProfileConfiguration
    : IEntityTypeConfiguration<JobSeekerProfile>
{
    public void Configure(EntityTypeBuilder<JobSeekerProfile> builder)
    {
        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(profile => profile.LastName).IsRequired().HasMaxLength(100);
        builder.Property(profile => profile.Phone).HasMaxLength(30);
        builder.Property(profile => profile.Location).HasMaxLength(150);
        builder.Property(profile => profile.Summary).HasMaxLength(2000);

        builder.HasIndex(profile => profile.UserId).IsUnique();

        builder.HasOne(profile => profile.User)
            .WithOne(user => user.JobSeekerProfile)
            .HasForeignKey<JobSeekerProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
