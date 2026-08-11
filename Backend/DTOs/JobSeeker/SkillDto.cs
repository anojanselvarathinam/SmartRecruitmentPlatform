namespace SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

public class SkillDto
{
    public int Id { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public string? SkillLevel { get; set; }
}
