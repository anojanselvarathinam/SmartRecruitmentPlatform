namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant
{
    public class ApplicantListDto
    {
        public int ApplicationId { get; set; }

        public int JobSeekerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public decimal MatchScore { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }
    }
}
