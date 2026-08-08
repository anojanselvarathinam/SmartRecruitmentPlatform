namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class CvDocument
{
    public int Id { get; set; }

    public int JobSeekerProfileId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public JobSeekerProfile? JobSeekerProfile { get; set; }
}