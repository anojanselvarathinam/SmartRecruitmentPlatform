using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ApplicationDbContext _context;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            ApplicationDbContext context)
        {
            _applicationRepository = applicationRepository;
            _context = context;
        }

        public async Task<IEnumerable<ApplicantListDto>>
    GetApplicantsByJobIdAsync(int jobId)
        {
            var applications =
                await _applicationRepository.GetByJobIdAsync(jobId);

            return applications
                .OrderByDescending(a => a.MatchScore)
                .Select(a => new ApplicantListDto
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

            var user =
                await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        u => u.UserId == application.JobSeekerId);

            return new ApplicantDetailsDto
            {
                ApplicationId = application.ApplicationId,
                JobId = application.JobId,
                JobSeekerId = application.JobSeekerId,
                FullName = user?.FullName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                MatchScore = application.MatchScore,
                Status = application.Status,
                AppliedAt = application.AppliedAt,
                CvFileName = null
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