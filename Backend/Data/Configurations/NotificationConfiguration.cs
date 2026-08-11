using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(notification => notification.NotificationId);
        builder.Property(notification => notification.Message)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(notification => notification.JobSeekerProfile)
            .WithMany(profile => profile.Notifications)
            .HasForeignKey(notification => notification.JobSeekerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(notification => new
        {
            notification.JobSeekerProfileId,
            notification.IsRead
        });
    }
}
