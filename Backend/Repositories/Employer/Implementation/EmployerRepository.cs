using Microsoft.EntityFrameworkCore;

using EmployerModel =
    SmartRecruitmentPlatform.Backend.Models.Employer.Employer;

using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerModel?> GetByIdAsync(int employerId)
        {
            return await _context.Employers
                .FirstOrDefaultAsync(e => e.EmployerId == employerId);
        }

        public async Task<EmployerModel?> GetByEmailAsync(string email)
        {
            return await _context.Employers
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<EmployerModel> CreateAsync(
            EmployerModel employer)
        {
            await _context.Employers.AddAsync(employer);

            await _context.SaveChangesAsync();

            return employer;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employers
                .AnyAsync(e => e.Email == email);
        }

        public async Task UpdateAsync(
            EmployerModel employer)
        {
            _context.Employers.Update(employer);

            await _context.SaveChangesAsync();
        }
    }
}