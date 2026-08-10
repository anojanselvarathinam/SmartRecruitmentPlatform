using SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int jobId);

        Task<IEnumerable<Job>> GetByCompanyIdAsync(int companyId);

        Task<IEnumerable<Job>> GetAllAsync();

        Task<Job> CreateAsync(Job job);

        Task UpdateAsync(Job job);
    }
}