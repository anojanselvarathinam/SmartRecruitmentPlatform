namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class JobSeekerProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Location { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<JobSeekerSkill> Skills { get; set; }
        = new List<JobSeekerSkill>();

    public ICollection<Education> Educations { get; set; }
        = new List<Education>();

    public ICollection<Experience> Experiences { get; set; }
        = new List<Experience>();

    public ICollection<CvDocument> CvDocuments { get; set; }
        = new List<CvDocument>();
}
