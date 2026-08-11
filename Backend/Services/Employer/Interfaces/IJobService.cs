using SmartRecruitmentPlatform.Backend.DTOs.Employer.Job;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IJobService
    {
        Task<JobResponseDto?> GetByIdAsync(int jobId, int employerId);

        Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(
            int companyId,
            int employerId);

        Task<JobResponseDto> CreateAsync(
            JobCreateDto dto,
            int employerId);

        Task<JobResponseDto?> UpdateAsync(
    int jobId,
    int employerId,
    JobUpdateDto dto);

        Task<bool> CloseJobAsync(
    int jobId,
    int employerId);
    }
}
