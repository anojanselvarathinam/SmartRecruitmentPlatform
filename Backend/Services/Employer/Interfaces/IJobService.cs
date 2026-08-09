using SmartRecruitmentPlatform.Backend.DTOs.Employer.Job;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IJobService
    {
        Task<JobResponseDto?> GetByIdAsync(int jobId);

        Task<IEnumerable<JobResponseDto>> GetByCompanyIdAsync(
            int companyId);

        Task<JobResponseDto> CreateAsync(
            JobCreateDto dto);

        Task<JobResponseDto?> UpdateAsync(
            int jobId,
            JobUpdateDto dto);

        Task<bool> CloseJobAsync(int jobId);
    }
}