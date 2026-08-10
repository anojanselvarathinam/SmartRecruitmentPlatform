using EmployerModel =
    SmartRecruitmentPlatform.Backend.Models.Employer.Employer;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces
{
    public interface IEmployerRepository
    {
        Task<EmployerModel?> GetByIdAsync(int employerId);

        Task<EmployerModel?> GetByEmailAsync(string email);

        Task<EmployerModel> CreateAsync(EmployerModel employer);

        Task<bool> EmailExistsAsync(string email);

        Task UpdateAsync(EmployerModel employer);
    }
}