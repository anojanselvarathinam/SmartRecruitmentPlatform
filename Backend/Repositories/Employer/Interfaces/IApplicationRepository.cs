using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int applicationId);

        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);

        Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<Application> CreateAsync(Application application);

        Task UpdateAsync(Application application);

        Task<bool> HasAppliedAsync(
            int jobId,
            int jobSeekerId);
    }
}