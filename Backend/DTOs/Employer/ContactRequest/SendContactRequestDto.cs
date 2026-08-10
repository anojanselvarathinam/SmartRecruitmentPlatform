namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest
{
    public class SendContactRequestDto
    {
        public int EmployerId { get; set; }

        public int JobSeekerId { get; set; }
    }
}