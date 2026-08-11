using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class JobSeekerService : IJobSeekerService
{
    private readonly IJobSeekerRepository _repository;

    public JobSeekerService(IJobSeekerRepository repository)
    {
        _repository = repository;
    }

    public async Task<JobSeekerProfileDto?> GetProfileAsync(int userId)
    {
        var profile = await _repository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        return MapToDto(profile);
    }

    public async Task<JobSeekerProfileDto> UpdateProfileAsync(
        int userId,
        UpdateJobSeekerProfileDto dto)
    {
        var profile = await _repository.GetProfileAsync(userId);

        if (profile == null)
        {
            profile = new JobSeekerProfile
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            profile = await _repository.CreateProfileAsync(profile);
        }

        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.Phone = dto.Phone;
        profile.Location = dto.Location;
        profile.Summary = dto.Summary;
        profile.UpdatedAt = DateTime.UtcNow;

        profile = await _repository.UpdateProfileAsync(profile);

        return MapToDto(profile);
    }

    private static JobSeekerProfileDto MapToDto(
        JobSeekerProfile profile)
    {
        return new JobSeekerProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Phone = profile.Phone,
            Location = profile.Location,
            Summary = profile.Summary,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,

            Skills = profile.Skills.Select(s => new SkillDto
            {
                Id = s.Id,
                SkillName = s.SkillName,
                SkillLevel = s.SkillLevel
            }).ToList(),

            Educations = profile.Educations.Select(e => new EducationDto
            {
                Id = e.Id,
                Institution = e.Institution,
                Degree = e.Degree,
                FieldOfStudy = e.FieldOfStudy,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            }).ToList(),

            Experiences = profile.Experiences.Select(e => new ExperienceDto
            {
                Id = e.Id,
                CompanyName = e.CompanyName,
                JobTitle = e.JobTitle,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            }).ToList(),

            CvDocuments = profile.CvDocuments.Select(c => new CvDto
            {
                Id = c.Id,
                FileName = c.FileName,
                ContentType = c.ContentType,
                FileSize = c.FileSize,
                UploadedAt = c.UploadedAt
            }).ToList()
        };
    }
}
