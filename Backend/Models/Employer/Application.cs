namespace SmartRecruitmentPlatform.Backend.Models.Employer
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        public decimal MatchScore { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Job Job { get; set; } = null!;

    }
}
