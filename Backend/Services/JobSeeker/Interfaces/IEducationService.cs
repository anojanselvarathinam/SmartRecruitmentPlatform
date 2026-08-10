using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface IEducationService
{
    Task<List<EducationDto>> GetEducationAsync(int userId);

    Task<EducationDto?> GetEducationByIdAsync(
        int userId,
        int educationId);

    Task<EducationDto?> AddEducationAsync(
        int userId,
        AddEducationDto dto);

    Task<EducationDto?> UpdateEducationAsync(
        int userId,
        int educationId,
        AddEducationDto dto);

    Task<bool> DeleteEducationAsync(
        int userId,
        int educationId);
}