using Microsoft.AspNetCore.Authorization;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Employer.Job;

namespace SmartRecruitmentPlatform.Backend.Controllers.Employer;
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employer")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJob(int jobId)
        {
            var job = await _jobService.GetByIdAsync(jobId);

            if (job == null)
                return NotFound("Job not found.");

            return Ok(job);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyJobs(int companyId)
        {
            var jobs = await _jobService.GetByCompanyIdAsync(companyId);

            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(
            [FromBody] JobCreateDto dto)
        {
            var job = await _jobService.CreateAsync(dto);

            return Ok(job);
        }

    [HttpPut("{jobId}")]
    public async Task<IActionResult> UpdateJob(
int jobId,
[FromBody] JobUpdateDto dto)
    {
        var employerIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

        if (!int.TryParse(employerIdClaim, out int employerId))
        {
            return Unauthorized("Employer ID not found in token.");
        }

        var job =
            await _jobService.UpdateAsync(
                jobId,
                employerId,
                dto);

        if (job == null)
            return NotFound("Job not found or access denied.");

        return Ok(job);
    }

    [HttpPut("{jobId}/close")]
    [HttpPut("{jobId}/close")]
    public async Task<IActionResult> CloseJob(int jobId)
    {
        var employerIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

        if (!int.TryParse(employerIdClaim, out int employerId))
        {
            return Unauthorized("Employer ID not found in token.");
        }

        var result =
            await _jobService.CloseJobAsync(
                jobId,
                employerId);

        if (!result)
            return NotFound("Job not found or access denied.");

        return Ok(new
        {
            message = "Job vacancy closed successfully."
        });
    }
}
