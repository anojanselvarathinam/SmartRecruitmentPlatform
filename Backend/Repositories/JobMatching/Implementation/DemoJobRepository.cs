using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using SmartRecruitmentPlatform.Backend.Models.JobMatching;
//using SmartRecruitmentPlatform.Backend.Repositories.JobMatching.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public sealed class DemoJobRepository : IJobRepository
{
    private static readonly IReadOnlyList<Job> Jobs = new List<Job>
    {
        new()
        {
            Id = 1,
            Title = "Junior .NET Developer",
            CompanyName = "Tech Lanka",
            Description = "Build and maintain ASP.NET Core APIs and internal web applications.",
            Location = "Colombo",
            RequiredExperienceYears = 1,
            RequiredEducation = "Bachelor Degree",
            RequiredSkills = new List<string> { "C#", "ASP.NET Core", "SQL", "Git" },
            IsOpen = true
        },
        new()
        {
            Id = 2,
            Title = "Frontend Developer",
            CompanyName = "Digital Works",
            Description = "Develop responsive interfaces for recruitment and business platforms.",
            Location = "Remote",
            RequiredExperienceYears = 1,
            RequiredEducation = "Diploma",
            RequiredSkills = new List<string> { "JavaScript", "HTML", "CSS", "Git" },
            IsOpen = true
        },
        new()
        {
            Id = 3,
            Title = "Backend Intern",
            CompanyName = "CodeHub Jaffna",
            Description = "Assist the backend team with APIs, SQL queries, bug fixing, and testing.",
            Location = "Jaffna",
            RequiredExperienceYears = 0,
            RequiredEducation = "Diploma",
            RequiredSkills = new List<string> { "C#", "SQL" },
            IsOpen = true
        },
        new()
        {
            Id = 4,
            Title = "Full Stack Developer",
            CompanyName = "NorthStar Software",
            Description = "Work across ASP.NET Core backend services and JavaScript frontend features.",
            Location = "Jaffna",
            RequiredExperienceYears = 2,
            RequiredEducation = "Bachelor Degree",
            RequiredSkills = new List<string> { "C#", "ASP.NET Core", "SQL", "JavaScript", "HTML", "CSS", "Git" },
            IsOpen = true
        },
        new()
        {
            Id = 5,
            Title = "Software Engineer - API",
            CompanyName = "Ocean Systems",
            Description = "Design REST APIs and database-backed services for enterprise products.",
            Location = "Colombo",
            RequiredExperienceYears = 3,
            RequiredEducation = "Bachelor Degree",
            RequiredSkills = new List<string> { "C#", "ASP.NET Core", "SQL", "Docker", "Git" },
            IsOpen = true
        },
        new()
        {
            Id = 6,
            Title = "Web Developer Trainee",
            CompanyName = "Pixel Labs",
            Description = "Entry-level web development role with training and mentorship.",
            Location = "Remote",
            RequiredExperienceYears = 0,
            RequiredEducation = "Diploma",
            RequiredSkills = new List<string> { "HTML", "CSS", "JavaScript" },
            IsOpen = true
        }
    };

    public Task<IReadOnlyList<Job>> SearchOpenJobsAsync(
        JobSearchRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Job> query = Jobs.Where(job => job.IsOpen);

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim();
            query = query.Where(job =>
                job.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                job.CompanyName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                job.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            var location = filter.Location.Trim();
            query = query.Where(job =>
                job.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Skill))
        {
            var skill = filter.Skill.Trim();
            query = query.Where(job =>
                job.RequiredSkills.Any(requiredSkill =>
                    requiredSkill.Contains(skill, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult<IReadOnlyList<Job>>(query.ToList());
    }

    public Task<Job?> GetByIdAsync(
        int jobId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Jobs.FirstOrDefault(job => job.Id == jobId));
    }
}
