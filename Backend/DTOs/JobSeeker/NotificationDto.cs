namespace SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

public class NotificationDto
{
    public int NotificationId { get; set; }
    public int? ApplicationId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
