using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class SkillService : ISkillService
{
    private readonly IJobSeekerRepository _profileRepository;
    private readonly ISkillRepository _skillRepository;

    public SkillService(
        IJobSeekerRepository profileRepository,
        ISkillRepository skillRepository)
    {
        _profileRepository = profileRepository;
        _skillRepository = skillRepository;
    }

    public async Task<List<SkillDto>> GetSkillsAsync(int userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return new List<SkillDto>();
        }

        var skills = await _skillRepository.GetSkillsAsync(profile.Id);

        return skills.Select(s => new SkillDto
        {
            Id = s.Id,
            SkillName = s.SkillName,
            SkillLevel = s.SkillLevel
        }).ToList();
    }

    public async Task<SkillDto?> GetSkillByIdAsync(
        int userId,
        int skillId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var skill = await _skillRepository.GetSkillByIdAsync(
            skillId,
            profile.Id);

        if (skill == null)
        {
            return null;
        }

        return new SkillDto
        {
            Id = skill.Id,
            SkillName = skill.SkillName,
            SkillLevel = skill.SkillLevel
        };
    }

    public async Task<SkillDto?> AddSkillAsync(
        int userId,
        AddSkillDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var skill = new JobSeekerSkill
        {
            JobSeekerProfileId = profile.Id,
            SkillName = dto.SkillName,
            SkillLevel = dto.SkillLevel
        };

        skill = await _skillRepository.AddSkillAsync(skill);

        return new SkillDto
        {
            Id = skill.Id,
            SkillName = skill.SkillName,
            SkillLevel = skill.SkillLevel
        };
    }

    public async Task<SkillDto?> UpdateSkillAsync(
        int userId,
        int skillId,
        AddSkillDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var skill = await _skillRepository.GetSkillByIdAsync(
            skillId,
            profile.Id);

        if (skill == null)
        {
            return null;
        }

        skill.SkillName = dto.SkillName;
        skill.SkillLevel = dto.SkillLevel;

        skill = await _skillRepository.UpdateSkillAsync(skill);

        return new SkillDto
        {
            Id = skill.Id,
            SkillName = skill.SkillName,
            SkillLevel = skill.SkillLevel
        };
    }

    public async Task<bool> DeleteSkillAsync(
        int userId,
        int skillId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return false;
        }

        var skill = await _skillRepository.GetSkillByIdAsync(
            skillId,
            profile.Id);

        if (skill == null)
        {
            return false;
        }

        return await _skillRepository.DeleteSkillAsync(skill);
    }
}
