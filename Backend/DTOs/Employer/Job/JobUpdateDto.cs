namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Job
{
    public class JobUpdateDto
    {
        public string JobTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RequiredSkills { get; set; } = string.Empty;

        public int RequiredExperience { get; set; }

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
