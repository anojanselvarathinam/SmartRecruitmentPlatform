namespace SmartRecruitmentPlatform.Backend.Models.JobSeeker;

public class JobSeekerSkill
{
    public int Id { get; set; }

    public int JobSeekerProfileId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public string? SkillLevel { get; set; }

    public JobSeekerProfile? JobSeekerProfile { get; set; }
}