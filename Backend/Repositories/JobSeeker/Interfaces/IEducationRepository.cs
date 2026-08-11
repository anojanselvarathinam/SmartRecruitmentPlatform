using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

public interface IEducationRepository
{
    Task<List<Education>> GetEducationAsync(int profileId);

    Task<Education?> GetEducationByIdAsync(int educationId, int profileId);

    Task<Education> AddEducationAsync(Education education);

    Task<Education> UpdateEducationAsync(Education education);

    Task<bool> DeleteEducationAsync(Education education);
}
