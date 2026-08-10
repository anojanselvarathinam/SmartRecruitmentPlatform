using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface ISkillService
{
    Task<List<SkillDto>> GetSkillsAsync(int userId);

    Task<SkillDto?> GetSkillByIdAsync(
        int userId,
        int skillId);

    Task<SkillDto?> AddSkillAsync(
        int userId,
        AddSkillDto dto);

    Task<SkillDto?> UpdateSkillAsync(
        int userId,
        int skillId,
        AddSkillDto dto);

    Task<bool> DeleteSkillAsync(
        int userId,
        int skillId);
}