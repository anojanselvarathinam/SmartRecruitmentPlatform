using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.Employer;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.MatchScore)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Applications
                .Where(a => a.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<Application> CreateAsync(Application application)
        {
            await _context.Applications.AddAsync(application);

            await _context.SaveChangesAsync();

            return application;
        }

        public async Task UpdateAsync(Application application)
        {
            _context.Applications.Update(application);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAppliedAsync(
            int jobId,
            int jobSeekerId)
        {
            return await _context.Applications
                .AnyAsync(a =>
                    a.JobId == jobId &&
                    a.JobSeekerId == jobSeekerId);
        }
    }
}