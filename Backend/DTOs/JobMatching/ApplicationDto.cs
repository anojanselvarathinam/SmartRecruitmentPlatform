namespace SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

public sealed class ApplicationDto
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal MatchScore { get; set; }
    public DateTime AppliedAtUtc { get; set; }
}
