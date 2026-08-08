using SmartRecruitmentPlatform.Backend.Models.JobMatching;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public sealed class DemoJobSeekerProfileRepository : IJobSeekerProfileRepository
{
    private static readonly IReadOnlyList<JobSeekerProfile> Profiles = new List<JobSeekerProfile>
    {
        new()
        {
            JobSeekerId = 1,
            FullName = "Demo Job Seeker",
            Location = "Jaffna",
            ExperienceYears = 1.5m,
            Education = "Bachelor Degree",
            Skills = new List<string>
            {
                "C#",
                "ASP.NET Core",
                "SQL",
                "Git",
                "HTML",
                "CSS",
                "JavaScript"
            }
        }
    };

    public Task<JobSeekerProfile?> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            Profiles.FirstOrDefault(profile => profile.JobSeekerId == jobSeekerId));
    }
}
