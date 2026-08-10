using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Data.Configurations;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(request => request.ContactRequestId);
        builder.Property(request => request.Status).IsRequired().HasMaxLength(30);

        builder.HasOne(request => request.Employer)
            .WithMany(employer => employer.ContactRequests)
            .HasForeignKey(request => request.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(request => request.JobSeeker)
            .WithMany(profile => profile.ContactRequests)
            .HasForeignKey(request => request.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
