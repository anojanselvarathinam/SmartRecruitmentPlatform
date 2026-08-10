using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data;

public class CvDocumentConfiguration : IEntityTypeConfiguration<CvDocument>
{
    public void Configure(EntityTypeBuilder<CvDocument> builder)
    {
        builder.HasKey(cv => cv.Id);
        builder.Property(cv => cv.FileName).IsRequired().HasMaxLength(255);
        builder.Property(cv => cv.FilePath).IsRequired().HasMaxLength(500);
        builder.Property(cv => cv.ContentType).IsRequired().HasMaxLength(100);

        builder.HasOne(cv => cv.JobSeekerProfile)
            .WithMany(profile => profile.CvDocuments)
            .HasForeignKey(cv => cv.JobSeekerProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
