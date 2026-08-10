namespace SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

public class SkillGapDto
{
    public int JobId { get; set; }

    public decimal MatchScore { get; set; }

    public List<string> MissingSkills { get; set; } = new();
}
