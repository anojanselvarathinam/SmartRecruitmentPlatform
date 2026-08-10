using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.Employer;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetByIdAsync(int companyId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }

        public async Task<Company?> GetByEmployerIdAsync(int employerId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.EmployerId == employerId);
        }

        public async Task<Company> CreateAsync(Company company)
        {
            await _context.Companies.AddAsync(company);

            await _context.SaveChangesAsync();

            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);

            await _context.SaveChangesAsync();
        }
    }
}