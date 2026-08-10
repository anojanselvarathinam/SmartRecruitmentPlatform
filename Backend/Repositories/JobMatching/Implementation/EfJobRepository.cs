using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.DTOs.JobMatching;
using MatchingJob = SmartRecruitmentPlatform.Backend.Models.JobMatching.Job;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public class EfJobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public EfJobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MatchingJob>> SearchOpenJobsAsync(
        JobSearchRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Jobs
            .AsNoTracking()
            .Include(job => job.Company)
            .Where(job => job.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim();
            query = query.Where(job =>
                job.JobTitle.Contains(keyword) ||
                job.Description.Contains(keyword) ||
                job.Company.CompanyName.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            var location = filter.Location.Trim();
            query = query.Where(job => job.Location.Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(filter.Skill))
        {
            var skill = filter.Skill.Trim();
            query = query.Where(job => job.RequiredSkills.Contains(skill));
        }

        var jobs = await query
            .OrderByDescending(job => job.CreatedAt)
            .ToListAsync(cancellationToken);

        return jobs.Select(MapJob).ToList();
    }

    public async Task<MatchingJob?> GetByIdAsync(
        int jobId,
        CancellationToken cancellationToken = default)
    {
        var job = await _context.Jobs
            .AsNoTracking()
            .Include(item => item.Company)
            .FirstOrDefaultAsync(
                item => item.JobId == jobId,
                cancellationToken);

        return job == null ? null : MapJob(job);
    }

    private static MatchingJob MapJob(Models.Job job)
    {
        return new MatchingJob
        {
            Id = job.JobId,
            Title = job.JobTitle,
            CompanyName = job.Company.CompanyName,
            Description = job.Description,
            Location = job.Location,
            RequiredExperienceYears = job.RequiredExperience,
            RequiredEducation = job.Education,
            RequiredSkills = SplitSkills(job.RequiredSkills),
            IsOpen = job.IsActive
        };
    }

    private static List<string> SplitSkills(string requiredSkills)
    {
        return requiredSkills
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(skill => skill.Trim())
            .Where(skill => skill.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
