using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using EmployerModel =
    SmartRecruitmentPlatform.Backend.Models.Employer;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(
            IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
        }

        public async Task<EmployerModel?> GetByIdAsync(
            int employerId)
        {
            return await _employerRepository
                .GetByIdAsync(employerId);
        }
    }
}