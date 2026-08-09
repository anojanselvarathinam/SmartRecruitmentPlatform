using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public interface IJobRepository
{
    Task<IReadOnlyList<Job>> SearchOpenJobsAsync(
        JobSearchRequestDto filter,
        CancellationToken cancellationToken = default);

    Task<Job?> GetByIdAsync(
        int jobId,
        CancellationToken cancellationToken = default);
}
