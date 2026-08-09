namespace SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

public sealed class JobSearchRequestDto
{
    public string? Keyword { get; set; }
    public string? Location { get; set; }
    public string? Skill { get; set; }
    public decimal? MinMatchScore { get; set; }
}
