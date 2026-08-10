using EmployerModel =
    SmartRecruitmentPlatform.Backend.Models.Employer.Employer;

using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

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