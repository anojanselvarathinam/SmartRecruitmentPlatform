using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Data;
using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            ApplicationDbContext context,
            INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ApplicantListDto>>
            GetApplicantsByJobIdAsync(int jobId, int employerId)
        {
            var ownsJob = await _context.Jobs
                .AnyAsync(job =>
                    job.JobId == jobId &&
                    job.Company.EmployerId == employerId);

            if (!ownsJob)
            {
                return new List<ApplicantListDto>();
            }

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
            GetApplicantDetailsAsync(int applicationId, int employerId)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            var ownsApplication = await _context.Jobs
                .AnyAsync(job =>
                    job.JobId == application.JobId &&
                    job.Company.EmployerId == employerId);

            if (!ownsApplication)
            {
                return null;
            }

            var user =
                await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        u => u.JobSeekerProfile != null &&
                             u.JobSeekerProfile.Id == application.JobSeekerId);

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
            int employerId,
            UpdateApplicationStatusDto dto)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return false;
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(item =>
                    item.JobId == application.JobId &&
                    item.Company.EmployerId == employerId);

            if (job == null)
            {
                return false;
            }

            var allowedStatuses = new[]
            {
                "Applied",
                "Reviewed",
                "Shortlisted",
                "Rejected",
                "Accepted"
            };

            var newStatus = allowedStatuses.FirstOrDefault(status =>
                status.Equals(dto.Status, StringComparison.OrdinalIgnoreCase));

            if (newStatus == null)
            {
                return false;
            }

            application.Status = newStatus;
            application.UpdatedAt = DateTime.UtcNow;

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _applicationRepository.UpdateAsync(application);

                await _notificationService.CreateApplicationStatusNotificationAsync(
                    application.JobSeekerId,
                    application.ApplicationId,
                    $"Your application for {job.JobTitle} was updated to {newStatus}.");

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return true;
        }
    }
}
