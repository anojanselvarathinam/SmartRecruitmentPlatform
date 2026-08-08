namespace SmartRecruitmentPlatform.Backend.Models.JobMatching;

public sealed class JobApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int JobSeekerId { get; set; }
    public string Status { get; set; } = "Applied";
    public decimal MatchScoreAtApplication { get; set; }
    public DateTime AppliedAtUtc { get; set; } = DateTime.UtcNow;
}
