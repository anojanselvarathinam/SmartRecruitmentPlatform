using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

public interface IExperienceRepository
{
    Task<List<Experience>> GetExperiencesAsync(int profileId);

    Task<Experience?> GetExperienceByIdAsync(int experienceId, int profileId);

    Task<Experience> AddExperienceAsync(Experience experience);

    Task<Experience> UpdateExperienceAsync(Experience experience);

    Task<bool> DeleteExperienceAsync(Experience experience);
}