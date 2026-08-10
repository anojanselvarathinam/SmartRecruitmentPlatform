using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(experience => experience.Id);
        builder.Property(experience => experience.CompanyName).IsRequired().HasMaxLength(200);
        builder.Property(experience => experience.JobTitle).IsRequired().HasMaxLength(150);
        builder.Property(experience => experience.Description).HasMaxLength(2000);

        builder.HasOne(experience => experience.JobSeekerProfile)
            .WithMany(profile => profile.Experiences)
            .HasForeignKey(experience => experience.JobSeekerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
