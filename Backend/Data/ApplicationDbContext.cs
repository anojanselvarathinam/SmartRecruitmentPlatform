using Microsoft.EntityFrameworkCore;

using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Authentication
        public DbSet<User> Users { get; set; }

        // Employer
        public DbSet<Employer> Employers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ContactRequest> ContactRequests { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employer → Company
            modelBuilder.Entity<Company>()
                .HasOne(c => c.Employer)
                .WithOne()
                .HasForeignKey<Company>(
                    c => c.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Company → Jobs
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Company)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Job → Applications
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employer → Contact Requests
            modelBuilder.Entity<ContactRequest>()
                .HasOne(c => c.Employer)
                .WithMany()
                .HasForeignKey(c => c.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employer Email unique
            modelBuilder.Entity<Employer>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Prevent duplicate application
            modelBuilder.Entity<Application>()
                .HasIndex(a => new
                {
                    a.JobId,
                    a.JobSeekerId
                })
                .IsUnique();
        }
    }
}