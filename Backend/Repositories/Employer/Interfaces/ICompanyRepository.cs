using SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int companyId);

        Task<Company?> GetByEmployerIdAsync(int employerId);

        Task<Company> CreateAsync(Company company);

        Task UpdateAsync(Company company);
    }
}