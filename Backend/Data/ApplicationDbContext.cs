using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Employer> Employers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ContactRequest> ContactRequests { get; set; }
        // Job Seeker

        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

        public DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<Experience> Experiences { get; set; }

        public DbSet<CvDocument> CvDocuments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}
