using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;

using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(user => user.Employer)
                .Include(user => user.JobSeekerProfile)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task RegisterUserAsync(
            User user,
            Employer? employer,
            JobSeekerProfile? jobSeekerProfile)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                if (employer != null)
                {
                    employer.UserId = user.UserId;
                    await _context.Employers.AddAsync(employer);
                }

                if (jobSeekerProfile != null)
                {
                    jobSeekerProfile.UserId = user.UserId;
                    await _context.JobSeekerProfiles.AddAsync(jobSeekerProfile);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
