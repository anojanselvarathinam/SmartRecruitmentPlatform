using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface IExperienceService
{
    Task<List<ExperienceDto>> GetExperiencesAsync(int userId);

    Task<ExperienceDto?> GetExperienceByIdAsync(
        int userId,
        int experienceId);

    Task<ExperienceDto?> AddExperienceAsync(
        int userId,
        AddExperienceDto dto);

    Task<ExperienceDto?> UpdateExperienceAsync(
        int userId,
        int experienceId,
        AddExperienceDto dto);

    Task<bool> DeleteExperienceAsync(
        int userId,
        int experienceId);
}