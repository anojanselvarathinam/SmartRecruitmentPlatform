using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<IEnumerable<ApplicantListDto>>
            GetApplicantsByJobIdAsync(int jobId)
        {
            var applications =
                await _applicationRepository.GetByJobIdAsync(jobId);

            return applications.Select(a => new ApplicantListDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                MatchScore = a.MatchScore,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            });
        }

        public async Task<ApplicantDetailsDto?>
            GetApplicantDetailsAsync(int applicationId)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            return new ApplicantDetailsDto
            {
                ApplicationId = application.ApplicationId,
                JobId = application.JobId,
                JobSeekerId = application.JobSeekerId,
                MatchScore = application.MatchScore,
                Status = application.Status,
                AppliedAt = application.AppliedAt
            };
        }

        public async Task<bool> UpdateStatusAsync(
            int applicationId,
            UpdateApplicationStatusDto dto)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return false;
            }

            application.Status = dto.Status;
            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.UpdateAsync(application);

            return true;
        }
    }
}