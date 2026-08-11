using SmartRecruitmentPlatform.Backend.DTOs.Employer.Company;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto?> GetByIdAsync(
            int companyId,
            int employerId);

        Task<CompanyResponseDto?> GetByEmployerIdAsync(
            int employerId);

        Task<CompanyResponseDto> CreateAsync(
    int employerId,
    CompanyCreateDto dto);

        Task<CompanyResponseDto?> UpdateAsync(
            int companyId,
            int employerId,
            CompanyUpdateDto dto);
    }
}
