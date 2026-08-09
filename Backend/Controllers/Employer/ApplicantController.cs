using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.Applicant;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

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
            var applicants =
                await _applicationService.GetApplicantsByJobIdAsync(jobId);

            return Ok(applicants);
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetApplicantDetails(
            int applicationId)
        {
            var applicant =
                await _applicationService.GetApplicantDetailsAsync(
                    applicationId);

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
            var result =
                await _applicationService.UpdateStatusAsync(
                    applicationId,
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
    }
}