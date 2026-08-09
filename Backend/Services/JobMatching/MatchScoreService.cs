using Microsoft.Extensions.Options;
using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public sealed class MatchScoreService : IMatchScoreService
{
    private readonly MatchingWeightOptions _weights;

    public MatchScoreService(IOptions<MatchingWeightOptions> options)
    {
        _weights = options.Value;

        if (_weights.Total != 100m)
        {
            throw new InvalidOperationException(
                $"Member 4 matching weights must total 100. Current total: {_weights.Total}.");
        }
    }

    public MatchResultDto Calculate(JobSeekerProfile profile, Job job)
    {
        var seekerSkills = profile.Skills
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Select(Normalize)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var requiredSkills = job.RequiredSkills
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Select(skill => skill.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var matchedSkills = requiredSkills
            .Where(skill => seekerSkills.Contains(Normalize(skill)))
            .ToList();

        var missingSkills = requiredSkills
            .Where(skill => !seekerSkills.Contains(Normalize(skill)))
            .ToList();

        decimal skillRatio = requiredSkills.Count == 0
            ? 1m
            : (decimal)matchedSkills.Count / requiredSkills.Count;

        decimal experienceRatio = job.RequiredExperienceYears <= 0m
            ? 1m
            : Math.Min(profile.ExperienceYears / job.RequiredExperienceYears, 1m);

        decimal educationRatio = GetEducationRatio(
            profile.Education,
            job.RequiredEducation);

        decimal locationRatio = IsLocationMatch(profile.Location, job.Location)
            ? 1m
            : 0m;

        var skillScore = Round(skillRatio * _weights.Skills);
        var experienceScore = Round(experienceRatio * _weights.Experience);
        var educationScore = Round(educationRatio * _weights.Education);
        var locationScore = Round(locationRatio * _weights.Location);

        var total = Math.Clamp(
            Round(skillScore + experienceScore + educationScore + locationScore),
            0m,
            100m);

        return new MatchResultDto
        {
            TotalScore = total,
            SkillScore = skillScore,
            ExperienceScore = experienceScore,
            EducationScore = educationScore,
            LocationScore = locationScore,
            MatchedSkills = matchedSkills,
            MissingSkills = missingSkills
        };
    }

    private static bool IsLocationMatch(string seekerLocation, string jobLocation)
    {
        var seeker = Normalize(seekerLocation);
        var job = Normalize(jobLocation);

        if (string.IsNullOrWhiteSpace(job) || job.Contains("remote"))
        {
            return true;
        }

        return seeker == job;
    }

    private static decimal GetEducationRatio(
        string seekerEducation,
        string requiredEducation)
    {
        if (string.IsNullOrWhiteSpace(requiredEducation))
        {
            return 1m;
        }

        var seekerLevel = EducationLevel(seekerEducation);
        var requiredLevel = EducationLevel(requiredEducation);

        // If free-text education is not recognized, use a simple text comparison.
        if (requiredLevel == 0)
        {
            return Normalize(seekerEducation).Contains(Normalize(requiredEducation))
                ? 1m
                : 0m;
        }

        if (seekerLevel >= requiredLevel)
        {
            return 1m;
        }

        if (seekerLevel > 0 && requiredLevel - seekerLevel == 1)
        {
            return 0.5m;
        }

        return 0m;
    }

    private static int EducationLevel(string value)
    {
        var normalized = Normalize(value);

        if (normalized.Contains("phd") || normalized.Contains("doctor"))
            return 4;

        if (normalized.Contains("master") ||
            normalized.Contains("msc") ||
            normalized.Contains("mba"))
            return 3;

        if (normalized.Contains("bachelor") ||
            normalized.Contains("bsc") ||
            normalized.Contains("degree"))
            return 2;

        if (normalized.Contains("diploma") ||
            normalized.Contains("hnd"))
            return 1;

        return 0;
    }

    private static decimal Round(decimal value) => Math.Round(value, 2);

    private static string Normalize(string? value) =>
        (value ?? string.Empty).Trim().ToLowerInvariant();
}
