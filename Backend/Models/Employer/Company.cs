namespace SmartRecruitmentPlatform.Backend.Models.Employer
{
    public class Company
    {
        public int CompanyId { get; set; }

        public int EmployerId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        // Navigation Properties
        public Employer Employer { get; set; } = null!;

        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
