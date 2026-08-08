using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.Company;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompany(int companyId)
        {
            var company = await _companyService.GetByIdAsync(companyId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }

        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetByEmployerId(int employerId)
        {
            var company =
                await _companyService.GetByEmployerIdAsync(employerId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(
            [FromBody] CompanyCreateDto dto)
        {
            var company =
                await _companyService.CreateAsync(dto);

            return Ok(company);
        }

        [HttpPut("{companyId}")]
        public async Task<IActionResult> UpdateCompany(
            int companyId,
            [FromBody] CompanyUpdateDto dto)
        {
            var company =
                await _companyService.UpdateAsync(
                    companyId,
                    dto);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }
    }
}