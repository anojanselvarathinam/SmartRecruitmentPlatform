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

    public SmartRecruitmentPlatform.Backend.Models.Authentication.User User { get; set; } = null!;

    public ICollection<JobSeekerSkill> Skills { get; set; }
        = new List<JobSeekerSkill>();

    public ICollection<Education> Educations { get; set; }
        = new List<Education>();

    public ICollection<Experience> Experiences { get; set; }
        = new List<Experience>();

    public ICollection<CvDocument> CvDocuments { get; set; }
        = new List<CvDocument>();

    public ICollection<SmartRecruitmentPlatform.Backend.Models.Application> Applications { get; set; }
        = new List<SmartRecruitmentPlatform.Backend.Models.Application>();

    public ICollection<SmartRecruitmentPlatform.Backend.Models.ContactRequest> ContactRequests { get; set; }
        = new List<SmartRecruitmentPlatform.Backend.Models.ContactRequest>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();
}
