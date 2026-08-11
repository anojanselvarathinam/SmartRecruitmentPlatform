using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(education => education.Id);
        builder.Property(education => education.Institution).IsRequired().HasMaxLength(200);
        builder.Property(education => education.Degree).IsRequired().HasMaxLength(150);
        builder.Property(education => education.FieldOfStudy).HasMaxLength(150);

        builder.HasOne(education => education.JobSeekerProfile)
            .WithMany(profile => profile.Educations)
            .HasForeignKey(education => education.JobSeekerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
