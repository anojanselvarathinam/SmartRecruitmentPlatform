namespace SmartRecruitmentPlatform.Backend.Models
{
    public class Employer
    {
        public int EmployerId { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Authentication.User User { get; set; } = null!;

        public Company? Company { get; set; }

        public ICollection<ContactRequest> ContactRequests { get; set; } =
            new List<ContactRequest>();
    }
}
