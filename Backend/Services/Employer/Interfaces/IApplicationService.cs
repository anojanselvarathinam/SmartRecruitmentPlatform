using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicantListDto>>
            GetApplicantsByJobIdAsync(int jobId, int employerId);

        Task<ApplicantDetailsDto?>
            GetApplicantDetailsAsync(int applicationId, int employerId);

        Task<bool> UpdateStatusAsync(
            int applicationId,
            int employerId,
            UpdateApplicationStatusDto dto);
    }
}
