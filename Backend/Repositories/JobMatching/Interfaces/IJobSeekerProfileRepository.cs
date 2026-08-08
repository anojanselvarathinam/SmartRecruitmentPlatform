using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public interface IJobSeekerProfileRepository
{
    Task<JobSeekerProfile?> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default);
}
