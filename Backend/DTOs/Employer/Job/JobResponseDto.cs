namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Job
{
    public class JobResponseDto
    {
        public int JobId { get; set; }

        public int CompanyId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RequiredSkills { get; set; } = string.Empty;

        public string RequiredExperience { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
