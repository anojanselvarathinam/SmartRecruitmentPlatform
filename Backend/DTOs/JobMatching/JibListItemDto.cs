namespace SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

public sealed class JobListItemDto
{
    public int JobId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal MatchScore { get; set; }
    public List<string> MissingSkills { get; set; } = new();
    public bool AlreadyApplied { get; set; }
}
