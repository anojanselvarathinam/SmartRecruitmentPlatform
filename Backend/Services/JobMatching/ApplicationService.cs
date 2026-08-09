using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public sealed class ApplicationService : IApplicationService
{
    private readonly IJobRepository _jobs;
    private readonly IJobSeekerProfileRepository _profiles;
    private readonly IApplicationRepository _applications;
    private readonly IMatchScoreService _matchScore;

    public ApplicationService(
        IJobRepository jobs,
        IJobSeekerProfileRepository profiles,
        IApplicationRepository applications,
        IMatchScoreService matchScore)
    {
        _jobs = jobs;
        _profiles = profiles;
        _applications = applications;
        _matchScore = matchScore;
    }

    public async Task<ApplyJobResult> ApplyAsync(
        int jobSeekerId,
        int jobId,
        CancellationToken cancellationToken = default)
    {
        var job = await _jobs.GetByIdAsync(jobId, cancellationToken);

        if (job is null)
        {
            return Failure("Job not found.");
        }

        if (!job.IsOpen)
        {
            return Failure("This vacancy is closed.");
        }

        // First duplicate check in the service layer.
        if (await _applications.ExistsAsync(jobId, jobSeekerId, cancellationToken))
        {
            return new ApplyJobResult
            {
                Success = false,
                IsDuplicate = true,
                Message = "You have already applied for this job."
            };
        }

        var profile = await _profiles.GetByJobSeekerIdAsync(
            jobSeekerId,
            cancellationToken);

        if (profile is null)
        {
            return Failure("Complete your job seeker profile before applying.");
        }

        var match = _matchScore.Calculate(profile, job);

        var application = new JobApplication
        {
            JobId = job.Id,
            JobSeekerId = jobSeekerId,
            Status = "Applied",
            MatchScoreAtApplication = match.TotalScore,
            AppliedAtUtc = DateTime.UtcNow
        };

        try
        {
            var saved = await _applications.AddAsync(
                application,
                cancellationToken);

            return new ApplyJobResult
            {
                Success = true,
                Message = "Application submitted successfully.",
                Application = new ApplicationDto
                {
                    ApplicationId = saved.Id,
                    JobId = saved.JobId,
                    JobTitle = job.Title,
                    CompanyName = job.CompanyName,
                    Status = saved.Status,
                    MatchScore = saved.MatchScoreAtApplication,
                    AppliedAtUtc = saved.AppliedAtUtc
                }
            };
        }
        catch (InvalidOperationException)
        {
            // Repository performs a second duplicate check for extra protection.
            return new ApplyJobResult
            {
                Success = false,
                IsDuplicate = true,
                Message = "You have already applied for this job."
            };
        }
    }

    public async Task<IReadOnlyList<ApplicationDto>> GetMyApplicationsAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        var applications = await _applications.GetByJobSeekerIdAsync(
            jobSeekerId,
            cancellationToken);

        var result = new List<ApplicationDto>();

        foreach (var application in applications)
        {
            var job = await _jobs.GetByIdAsync(application.JobId, cancellationToken);

            result.Add(new ApplicationDto
            {
                ApplicationId = application.Id,
                JobId = application.JobId,
                JobTitle = job?.Title ?? $"Job #{application.JobId}",
                CompanyName = job?.CompanyName ?? string.Empty,
                Status = application.Status,
                MatchScore = application.MatchScoreAtApplication,
                AppliedAtUtc = application.AppliedAtUtc
            });
        }

        return result;
    }

    private static ApplyJobResult Failure(string message) => new()
    {
        Success = false,
        Message = message
    };
}
