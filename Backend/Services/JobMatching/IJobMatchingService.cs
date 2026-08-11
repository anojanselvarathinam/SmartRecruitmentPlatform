using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public interface IJobMatchingService
{
    Task<IReadOnlyList<JobListItemDto>> SearchAsync(
        int jobSeekerId,
        JobSearchRequestDto filter,
        CancellationToken cancellationToken = default);

    Task<JobDetailsDto?> GetDetailsAsync(
        int jobSeekerId,
        int jobId,
        CancellationToken cancellationToken = default);

    Task<DemoProfileDto?> GetDemoProfileAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default);
}
