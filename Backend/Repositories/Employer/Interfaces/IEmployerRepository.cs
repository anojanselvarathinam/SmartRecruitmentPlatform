using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IEmployerRepository
    {
        Task<Employer?> GetByIdAsync(int employerId);

        Task<Employer?> GetByEmailAsync(string email);

        Task<Employer> CreateAsync(Employer employer);

        Task<bool> EmailExistsAsync(string email);

        Task UpdateAsync(Employer employer);
    }
}