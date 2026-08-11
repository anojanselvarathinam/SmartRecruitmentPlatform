using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

public interface IJobSeekerRepository
{
    Task<JobSeekerProfile?> GetProfileAsync(int userId);

    Task<JobSeekerProfile?> GetProfileByIdAsync(int profileId);

    Task<JobSeekerProfile> CreateProfileAsync(JobSeekerProfile profile);

    Task<JobSeekerProfile> UpdateProfileAsync(JobSeekerProfile profile);
}
