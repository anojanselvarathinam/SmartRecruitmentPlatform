using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class JobSeekerSkillConfiguration
    : IEntityTypeConfiguration<JobSeekerSkill>
{
    public void Configure(EntityTypeBuilder<JobSeekerSkill> builder)
    {
        builder.HasKey(skill => skill.Id);
        builder.Property(skill => skill.SkillName).IsRequired().HasMaxLength(100);
        builder.Property(skill => skill.SkillLevel).HasMaxLength(50);

        builder.HasIndex(skill => new { skill.JobSeekerProfileId, skill.SkillName })
            .IsUnique();

        builder.HasOne(skill => skill.JobSeekerProfile)
            .WithMany(profile => profile.Skills)
            .HasForeignKey(skill => skill.JobSeekerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
