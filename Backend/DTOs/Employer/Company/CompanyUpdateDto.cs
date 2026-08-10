namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Company
{
    public class CompanyUpdateDto
    {
        public string CompanyName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;
    }
}