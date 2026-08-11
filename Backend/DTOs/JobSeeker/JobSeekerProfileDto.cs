namespace SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

public class JobSeekerProfileDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Location { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<SkillDto> Skills { get; set; } = new();

    public List<EducationDto> Educations { get; set; } = new();

    public List<ExperienceDto> Experiences { get; set; } = new();

    public List<CvDto> CvDocuments { get; set; } = new();
}
