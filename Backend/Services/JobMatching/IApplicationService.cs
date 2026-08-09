using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public interface IApplicationService
{
    Task<ApplyJobResult> ApplyAsync(
        int jobSeekerId,
        int jobId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationDto>> GetMyApplicationsAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default);
}

public sealed class ApplyJobResult
{
    public bool Success { get; init; }
    public bool IsDuplicate { get; init; }
    public string Message { get; init; } = string.Empty;
    public ApplicationDto? Application { get; init; }
}
