namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest
{
    public class ContactRequestResponseDto
    {
        public int ContactRequestId { get; set; }

        public int EmployerId { get; set; }

        public int JobSeekerId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? RespondedAt { get; set; }
    }
}