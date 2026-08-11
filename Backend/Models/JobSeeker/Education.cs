namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class Education
{
    public int Id { get; set; }

    public int JobSeekerProfileId { get; set; }

    public string Institution { get; set; } = string.Empty;

    public string Degree { get; set; } = string.Empty;

    public string? FieldOfStudy { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public JobSeekerProfile? JobSeekerProfile { get; set; }
}