using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Implementations
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employer?> GetByIdAsync(int employerId)
        {
            return await _context.Employers
                .FirstOrDefaultAsync(e => e.EmployerId == employerId);
        }

        public async Task<Employer?> GetByEmailAsync(string email)
        {
            return await _context.Employers
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employer> CreateAsync(Employer employer)
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

        public async Task UpdateAsync(Employer employer)
        {
            _context.Employers.Update(employer);
            await _context.SaveChangesAsync();
        }
    }
}