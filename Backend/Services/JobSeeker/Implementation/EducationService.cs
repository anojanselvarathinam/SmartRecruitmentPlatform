using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class EducationService : IEducationService
{
    private readonly IJobSeekerRepository _profileRepository;
    private readonly IEducationRepository _educationRepository;

    public EducationService(
        IJobSeekerRepository profileRepository,
        IEducationRepository educationRepository)
    {
        _profileRepository = profileRepository;
        _educationRepository = educationRepository;
    }

    public async Task<List<EducationDto>> GetEducationAsync(int userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return new List<EducationDto>();
        }

        var education = await _educationRepository
            .GetEducationAsync(profile.Id);

        return education.Select(e => new EducationDto
        {
            Id = e.Id,
            Institution = e.Institution,
            Degree = e.Degree,
            FieldOfStudy = e.FieldOfStudy,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList();
    }

    public async Task<EducationDto?> GetEducationByIdAsync(
        int userId,
        int educationId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var education = await _educationRepository
            .GetEducationByIdAsync(educationId, profile.Id);

        if (education == null)
        {
            return null;
        }

        return MapToDto(education);
    }

    public async Task<EducationDto?> AddEducationAsync(
        int userId,
        AddEducationDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var education = new Education
        {
            JobSeekerProfileId = profile.Id,
            Institution = dto.Institution,
            Degree = dto.Degree,
            FieldOfStudy = dto.FieldOfStudy,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        education = await _educationRepository
            .AddEducationAsync(education);

        return MapToDto(education);
    }

    public async Task<EducationDto?> UpdateEducationAsync(
        int userId,
        int educationId,
        AddEducationDto dto)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var education = await _educationRepository
            .GetEducationByIdAsync(educationId, profile.Id);

        if (education == null)
        {
            return null;
        }

        education.Institution = dto.Institution;
        education.Degree = dto.Degree;
        education.FieldOfStudy = dto.FieldOfStudy;
        education.StartDate = dto.StartDate;
        education.EndDate = dto.EndDate;

        education = await _educationRepository
            .UpdateEducationAsync(education);

        return MapToDto(education);
    }

    public async Task<bool> DeleteEducationAsync(
        int userId,
        int educationId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return false;
        }

        var education = await _educationRepository
            .GetEducationByIdAsync(educationId, profile.Id);

        if (education == null)
        {
            return false;
        }

        return await _educationRepository
            .DeleteEducationAsync(education);
    }

    private static EducationDto MapToDto(Education education)
    {
        return new EducationDto
        {
            Id = education.Id,
            Institution = education.Institution,
            Degree = education.Degree,
            FieldOfStudy = education.FieldOfStudy,
            StartDate = education.StartDate,
            EndDate = education.EndDate
        };
    }
}