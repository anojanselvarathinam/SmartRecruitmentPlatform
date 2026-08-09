using EmployerModel =
    SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerModel?> GetByIdAsync(int employerId);
    }
}