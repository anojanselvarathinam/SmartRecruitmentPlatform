namespace SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

public class UpdateJobSeekerProfileDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Location { get; set; }

    public string? Summary { get; set; }
}
