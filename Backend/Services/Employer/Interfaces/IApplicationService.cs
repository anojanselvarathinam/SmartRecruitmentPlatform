using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;

namespace SmartRecruitmentPlatform.Backend.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicantListDto>>
            GetApplicantsByJobIdAsync(int jobId);

        Task<ApplicantDetailsDto?>
            GetApplicantDetailsAsync(int applicationId);

        Task<bool> UpdateStatusAsync(
            int applicationId,
            UpdateApplicationStatusDto dto);
    }
}