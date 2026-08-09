namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant
{
    public class ApplicantDetailsDto
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public decimal MatchScore { get; set; }

        public DateTime AppliedAt { get; set; }

        public string? CvFileName { get; set; }
    }
}
