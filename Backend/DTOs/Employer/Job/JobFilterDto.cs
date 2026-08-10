namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Job
{
    public class JobFilterDto
    {
        public string? SearchTerm { get; set; }

        public string? Location { get; set; }

        public string? RequiredSkills { get; set; }

        public bool? IsActive { get; set; }
    }
}
