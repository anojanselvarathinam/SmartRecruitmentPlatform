namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class Experience
{
    public int Id { get; set; }

    public int JobSeekerProfileId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public JobSeekerProfile? JobSeekerProfile { get; set; }
}