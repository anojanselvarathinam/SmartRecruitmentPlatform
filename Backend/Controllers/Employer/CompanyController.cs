using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.Company;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

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
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var company = await _companyService.GetByIdAsync(
                companyId,
                employerId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }

        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetByEmployerId(int employerId)
        {
            if (!TryGetEmployerId(out int authenticatedEmployerId) ||
                authenticatedEmployerId != employerId)
            {
                return Forbid();
            }

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
            if (!TryGetEmployerId(out int employerId))
            {
                return Unauthorized("Employer ID not found in token.");
            }

            var company =
                await _companyService.CreateAsync(
                    employerId,
                    dto);

            return Ok(company);
        }

        [HttpPut("{companyId}")]
        public async Task<IActionResult> UpdateCompany(
            int companyId,
            [FromBody] CompanyUpdateDto dto)
        {
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var company =
                await _companyService.UpdateAsync(
                    companyId,
                    employerId,
                    dto);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(company);
        }

        private bool TryGetEmployerId(out int employerId)
        {
            var employerIdClaim = User.FindFirst("employerId")?.Value;
            return int.TryParse(employerIdClaim, out employerId);
        }
    }
}
