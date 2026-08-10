using SmartRecruitmentPlatform.Backend.DTOs.Employer.Company;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto?> GetByIdAsync(
            int companyId);

        Task<CompanyResponseDto?> GetByEmployerIdAsync(
            int employerId);

        Task<CompanyResponseDto> CreateAsync(
            CompanyCreateDto dto);

        Task<CompanyResponseDto?> UpdateAsync(
            int companyId,
            CompanyUpdateDto dto);
    }
}