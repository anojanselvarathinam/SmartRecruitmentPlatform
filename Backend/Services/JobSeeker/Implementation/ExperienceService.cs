using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class ExperienceService : IExperienceService
{
    private readonly IJobSeekerRepository _profileRepository;
    private readonly IExperienceRepository _experienceRepository;

    public ExperienceService(
        IJobSeekerRepository profileRepository,
        IExperienceRepository experienceRepository)
    {
        _profileRepository = profileRepository;
        _experienceRepository = experienceRepository;
    }

    public async Task<List<ExperienceDto>> GetExperiencesAsync(int userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return new List<ExperienceDto>();
        }

        var experiences = await _experienceRepository
            .GetExperiencesAsync(profile.Id);

        return experiences.Select(MapToDto).ToList();
    }

    public async Task<ExperienceDto?> GetExperienceByIdAsync(
        int userId,
        int experienceId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var experience = await _experienceRepository
            .GetExperienceByIdAsync(experienceId, profile.Id);

        if (experience == null)
        {
            return null;
        }

        return MapToDto(experience);
    }

    public async Task<ExperienceDto?> AddExperienceAsync(
        int userId,
        AddExperienceDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var experience = new Experience
        {
            JobSeekerProfileId = profile.Id,
            CompanyName = dto.CompanyName,
            JobTitle = dto.JobTitle,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        experience = await _experienceRepository
            .AddExperienceAsync(experience);

        return MapToDto(experience);
    }

    public async Task<ExperienceDto?> UpdateExperienceAsync(
        int userId,
        int experienceId,
        AddExperienceDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var experience = await _experienceRepository
            .GetExperienceByIdAsync(experienceId, profile.Id);

        if (experience == null)
        {
            return null;
        }

        experience.CompanyName = dto.CompanyName;
        experience.JobTitle = dto.JobTitle;
        experience.Description = dto.Description;
        experience.StartDate = dto.StartDate;
        experience.EndDate = dto.EndDate;

        experience = await _experienceRepository
            .UpdateExperienceAsync(experience);

        return MapToDto(experience);
    }

    public async Task<bool> DeleteExperienceAsync(
        int userId,
        int experienceId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return false;
        }

        var experience = await _experienceRepository
            .GetExperienceByIdAsync(experienceId, profile.Id);

        if (experience == null)
        {
            return false;
        }

        return await _experienceRepository
            .DeleteExperienceAsync(experience);
    }

    private static ExperienceDto MapToDto(
        Experience experience)
    {
        return new ExperienceDto
        {
            Id = experience.Id,
            CompanyName = experience.CompanyName,
            JobTitle = experience.JobTitle,
            Description = experience.Description,
            StartDate = experience.StartDate,
            EndDate = experience.EndDate
        };
    }
}