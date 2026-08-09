namespace SmartRecruitmentPlatform.Backend.Models.Employer
{
    public class Employer
    {
        public int EmployerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public Company? Company { get; set; }
    }
}
