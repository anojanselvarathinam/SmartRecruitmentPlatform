namespace SmartRecruitmentPlatform.Backend.DTOs.JobMatching;

public sealed class JobDetailsDto
{
    public int JobId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal RequiredExperienceYears { get; set; }
    public string RequiredEducation { get; set; } = string.Empty;
    public List<string> RequiredSkills { get; set; } = new();
    public MatchResultDto Match { get; set; } = new();
    public bool AlreadyApplied { get; set; }
}
