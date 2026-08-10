using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.Employer;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<IEnumerable<Job>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Jobs
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Where(j => j.IsActive)
                .ToListAsync();
        }

        public async Task<Job> CreateAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);

            await _context.SaveChangesAsync();

            return job;
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);

            await _context.SaveChangesAsync();
        }
    }
}