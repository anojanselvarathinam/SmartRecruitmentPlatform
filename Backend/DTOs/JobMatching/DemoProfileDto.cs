namespace SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

public sealed class DemoProfileDto
{
    public int JobSeekerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal ExperienceYears { get; set; }
    public string Education { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
}
