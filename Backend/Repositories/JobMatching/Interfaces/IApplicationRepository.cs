using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public interface IApplicationRepository
{
    Task<bool> ExistsAsync(
        int jobId,
        int jobSeekerId,
        CancellationToken cancellationToken = default);

    Task<JobApplication> AddAsync(
        JobApplication application,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobApplication>> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default);
}
