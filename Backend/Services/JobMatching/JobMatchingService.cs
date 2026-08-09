using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public sealed class JobMatchingService : IJobMatchingService
{
    private readonly IJobRepository _jobs;
    private readonly IJobSeekerProfileRepository _profiles;
    private readonly IApplicationRepository _applications;
    private readonly IMatchScoreService _matchScore;

    public JobMatchingService(
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

    public async Task<IReadOnlyList<JobListItemDto>> SearchAsync(
        int jobSeekerId,
        JobSearchRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profiles.GetByJobSeekerIdAsync(
            jobSeekerId,
            cancellationToken);

        if (profile is null)
        {
            throw new InvalidOperationException(
                "Job seeker profile was not found. Complete the profile before matching.");
        }

        var jobs = await _jobs.SearchOpenJobsAsync(filter, cancellationToken);
        var result = new List<JobListItemDto>();

        foreach (var job in jobs)
        {
            var match = _matchScore.Calculate(profile, job);

            if (filter.MinMatchScore.HasValue &&
                match.TotalScore < filter.MinMatchScore.Value)
            {
                continue;
            }

            var alreadyApplied = await _applications.ExistsAsync(
                job.Id,
                jobSeekerId,
                cancellationToken);

            result.Add(new JobListItemDto
            {
                JobId = job.Id,
                Title = job.Title,
                CompanyName = job.CompanyName,
                Location = job.Location,
                MatchScore = match.TotalScore,
                MissingSkills = match.MissingSkills,
                AlreadyApplied = alreadyApplied
            });
        }

        return result
            .OrderByDescending(item => item.MatchScore)
            .ThenBy(item => item.Title)
            .ToList();
    }

    public async Task<JobDetailsDto?> GetDetailsAsync(
        int jobSeekerId,
        int jobId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profiles.GetByJobSeekerIdAsync(
            jobSeekerId,
            cancellationToken);

        if (profile is null)
        {
            throw new InvalidOperationException("Job seeker profile was not found.");
        }

        var job = await _jobs.GetByIdAsync(jobId, cancellationToken);
        if (job is null || !job.IsOpen)
        {
            return null;
        }

        var match = _matchScore.Calculate(profile, job);
        var alreadyApplied = await _applications.ExistsAsync(
            job.Id,
            jobSeekerId,
            cancellationToken);

        return new JobDetailsDto
        {
            JobId = job.Id,
            Title = job.Title,
            CompanyName = job.CompanyName,
            Description = job.Description,
            Location = job.Location,
            RequiredExperienceYears = job.RequiredExperienceYears,
            RequiredEducation = job.RequiredEducation,
            RequiredSkills = job.RequiredSkills,
            Match = match,
            AlreadyApplied = alreadyApplied
        };
    }

    public async Task<DemoProfileDto?> GetDemoProfileAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profiles.GetByJobSeekerIdAsync(
            jobSeekerId,
            cancellationToken);

        if (profile is null)
        {
            return null;
        }

        return new DemoProfileDto
        {
            JobSeekerId = profile.JobSeekerId,
            FullName = profile.FullName,
            Location = profile.Location,
            ExperienceYears = profile.ExperienceYears,
            Education = profile.Education,
            Skills = profile.Skills
        };
    }
}
