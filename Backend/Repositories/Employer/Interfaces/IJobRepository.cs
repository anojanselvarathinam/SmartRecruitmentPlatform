using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int jobId);

        Task<IEnumerable<Job>> GetByCompanyIdAsync(
            int companyId);

        Task<IEnumerable<Job>> GetAllAsync();

        Task<Job> CreateAsync(Job job);

        Task UpdateAsync(Job job);
    }
}