namespace SmartRecruitmentPlatform.Backend.DTOs.Employer.Auth
{
    public class EmployerRegisterDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}