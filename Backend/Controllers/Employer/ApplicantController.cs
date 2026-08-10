using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicantController(
            IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicants(int jobId)
        {
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var applicants =
                await _applicationService.GetApplicantsByJobIdAsync(
                    jobId,
                    employerId);

            return Ok(applicants);
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetApplicantDetails(
            int applicationId)
        {
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var applicant =
                await _applicationService.GetApplicantDetailsAsync(
                    applicationId,
                    employerId);

            if (applicant == null)
            {
                return NotFound("Applicant not found.");
            }

            return Ok(applicant);
        }

        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            [FromBody] UpdateApplicationStatusDto dto)
        {
            if (!TryGetEmployerId(out int employerId))
                return Unauthorized("Employer ID not found in token.");

            var result =
                await _applicationService.UpdateStatusAsync(
                    applicationId,
                    employerId,
                    dto);

            if (!result)
            {
                return NotFound("Application not found.");
            }

            return Ok(new
            {
                message = "Application status updated successfully."
            });
        }

        private bool TryGetEmployerId(out int employerId)
        {
            var employerIdClaim = User.FindFirst("employerId")?.Value;
            return int.TryParse(employerIdClaim, out employerId);
        }
    }
}
