using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Services.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Controllers.JobMatching;

[ApiController]
[Route("api/job-matching")]
public sealed class JobMatchingController : ControllerBase
{
    private readonly IJobMatchingService _jobMatchingService;
    private readonly IApplicationService _applicationService;
    private readonly IConfiguration _configuration;

    public JobMatchingController(
        IJobMatchingService jobMatchingService,
        IApplicationService applicationService,
        IConfiguration configuration)
    {
        _jobMatchingService = jobMatchingService;
        _applicationService = applicationService;
        _configuration = configuration;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "ok",
            module = "Member 4 - Job Matching & Applications",
            timeUtc = DateTime.UtcNow
        });
    }

    [HttpGet("demo-profile")]
    public async Task<IActionResult> GetDemoProfile(CancellationToken cancellationToken)
    {
        var profile = await _jobMatchingService.GetDemoProfileAsync(
            GetDemoJobSeekerId(),
            cancellationToken);

        return profile is null
            ? NotFound(new { message = "Demo profile not found." })
            : Ok(profile);
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> SearchJobs(
        [FromQuery] JobSearchRequestDto filter,
        CancellationToken cancellationToken)
    {
        if (filter.MinMatchScore.HasValue &&
            (filter.MinMatchScore.Value < 0m || filter.MinMatchScore.Value > 100m))
        {
            return BadRequest(new { message = "Minimum match score must be between 0 and 100." });
        }

        try
        {
            var result = await _jobMatchingService.SearchAsync(
                GetDemoJobSeekerId(),
                filter,
                cancellationToken);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("jobs/{jobId:int}")]
    public async Task<IActionResult> GetJobDetails(
        int jobId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _jobMatchingService.GetDetailsAsync(
                GetDemoJobSeekerId(),
                jobId,
                cancellationToken);

            return result is null
                ? NotFound(new { message = "Job not found or vacancy is closed." })
                : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("jobs/{jobId:int}/apply")]
    public async Task<IActionResult> Apply(
        int jobId,
        CancellationToken cancellationToken)
    {
        var result = await _applicationService.ApplyAsync(
            GetDemoJobSeekerId(),
            jobId,
            cancellationToken);

        if (result.Success)
        {
            return Ok(new
            {
                message = result.Message,
                application = result.Application
            });
        }

        if (result.IsDuplicate)
        {
            return Conflict(new { message = result.Message });
        }

        return BadRequest(new { message = result.Message });
    }

    [HttpGet("applications")]
    public async Task<IActionResult> GetMyApplications(
        CancellationToken cancellationToken)
    {
        var result = await _applicationService.GetMyApplicationsAsync(
            GetDemoJobSeekerId(),
            cancellationToken);

        return Ok(result);
    }

    private int GetDemoJobSeekerId()
    {
        return _configuration.GetValue<int?>("Member4Demo:JobSeekerId") ?? 1;
    }
}
