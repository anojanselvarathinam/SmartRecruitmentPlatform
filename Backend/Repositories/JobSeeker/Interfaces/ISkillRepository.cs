using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

public interface ISkillRepository
{
    Task<List<JobSeekerSkill>> GetSkillsAsync(int profileId);

    Task<JobSeekerSkill?> GetSkillByIdAsync(int skillId, int profileId);

    Task<JobSeekerSkill> AddSkillAsync(JobSeekerSkill skill);

    Task<JobSeekerSkill> UpdateSkillAsync(JobSeekerSkill skill);

    Task<bool> DeleteSkillAsync(JobSeekerSkill skill);
}
