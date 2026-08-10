using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface IJobSeekerService
{
    Task<JobSeekerProfileDto?> GetProfileAsync(int userId);

    Task<JobSeekerProfileDto> UpdateProfileAsync(
        int userId,
        UpdateJobSeekerProfileDto dto);
}