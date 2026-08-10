using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int companyId);

        Task<Company?> GetByEmployerIdAsync(int employerId);

        Task<Company> CreateAsync(Company company);

        Task UpdateAsync(Company company);
    }
}