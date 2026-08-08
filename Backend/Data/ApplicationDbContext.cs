using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Models.Authentication;

namespace SmartRecruitmentPlatform.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}