using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
//using SmartRecruitmentPlatform.Backend.Models.Employer;
//using SmartRecruitmentPlatform.Backend.Models.JobMatching;
//using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

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

        //public DbSet<Employer> Employers { get; set; }

        //public DbSet<JobSeeker> JobSeekers { get; set; }

        //public DbSet<Job> Jobs { get; set; }

        //public DbSet<Application> Applications { get; set; }
    }
}