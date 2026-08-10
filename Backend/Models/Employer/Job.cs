namespace SmartRecruitmentPlatform.Backend.Models.Employer
{
    public class Job
    {
        public int JobId { get; set; }

        public int CompanyId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RequiredSkills { get; set; } = string.Empty;

        public string RequiredExperience { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public Company? Company { get; set; }

        public List<Application> Applications { get; set; }
            = new List<Application>();
    }
}