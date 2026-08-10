namespace SmartRecruitmentPlatform.Backend.Models
{
    public class ContactRequest
    {
        public int ContactRequestId { get; set; }

        public int EmployerId { get; set; }

        public int JobSeekerId { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        // Navigation Properties
        public Employer Employer { get; set; } = null!;

        public JobSeeker.JobSeekerProfile JobSeeker { get; set; } = null!;
    }
}
