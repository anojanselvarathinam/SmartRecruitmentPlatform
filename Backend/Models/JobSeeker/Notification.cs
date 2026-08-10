namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class Notification
{
    public int NotificationId { get; set; }
    public int JobSeekerProfileId { get; set; }
    public int? ApplicationId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public JobSeekerProfile JobSeekerProfile { get; set; } = null!;
}
