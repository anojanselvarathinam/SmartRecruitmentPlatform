using SmartRecruitmentPlatform.Backend.DTOs.Employer.Company;
using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(
            int companyId)
        {
            var company =
                await _companyRepository.GetByIdAsync(companyId);

            if (company == null)
            {
                return null;
            }

            return MapToResponse(company);
        }

        public async Task<CompanyResponseDto?> GetByEmployerIdAsync(
            int employerId)
        {
            var company =
                await _companyRepository.GetByEmployerIdAsync(employerId);

            if (company == null)
            {
                return null;
            }

            return MapToResponse(company);
        }

        public async Task<CompanyResponseDto> CreateAsync(
    int employerId,
    CompanyCreateDto dto)
        {
            var company = new Company
            {
                EmployerId = employerId,
                CompanyName = dto.CompanyName,
                Description = dto.Description,
                Location = dto.Location,
                Industry = dto.Industry,
                Website = dto.Website
            };

            var created =
                await _companyRepository.CreateAsync(company);

            return MapToResponse(created);
        }

        public async Task<CompanyResponseDto?> UpdateAsync(
            int companyId,
            CompanyUpdateDto dto)
        {
            var company =
                await _companyRepository.GetByIdAsync(companyId);

            if (company == null)
            {
                return null;
            }

            company.CompanyName = dto.CompanyName;
            company.Description = dto.Description;
            company.Location = dto.Location;
            company.Industry = dto.Industry;
            company.Website = dto.Website;

            await _companyRepository.UpdateAsync(company);

            return MapToResponse(company);
        }

        private static CompanyResponseDto MapToResponse(
            Company company)
        {
            return new CompanyResponseDto
            {
                CompanyId = company.CompanyId,
                EmployerId = company.EmployerId,
                CompanyName = company.CompanyName,
                Description = company.Description,
                Location = company.Location,
                Industry = company.Industry,
                Website = company.Website
            };
        }
    }
}