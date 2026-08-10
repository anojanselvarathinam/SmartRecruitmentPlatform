using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using MatchingApplication = SmartRecruitmentPlatform.Backend.Models.JobMatching.JobApplication;
using DatabaseApplication = SmartRecruitmentPlatform.Backend.Models.Application;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public class EfApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public EfApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(
        int jobId,
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        return _context.Applications.AnyAsync(
            application =>
                application.JobId == jobId &&
                application.JobSeekerId == jobSeekerId,
            cancellationToken);
    }

    public async Task<MatchingApplication> AddAsync(
        MatchingApplication application,
        CancellationToken cancellationToken = default)
    {
        var alreadyExists = await ExistsAsync(
            application.JobId,
            application.JobSeekerId,
            cancellationToken);

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                "Duplicate application is not allowed for the same vacancy.");
        }

        var databaseApplication = new DatabaseApplication
        {
            JobId = application.JobId,
            JobSeekerId = application.JobSeekerId,
            MatchScore = application.MatchScoreAtApplication,
            Status = application.Status,
            AppliedAt = application.AppliedAtUtc
        };

        await _context.Applications.AddAsync(
            databaseApplication,
            cancellationToken);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException(
                "Duplicate application is not allowed for the same vacancy.");
        }

        return MapApplication(databaseApplication);
    }

    public async Task<IReadOnlyList<MatchingApplication>> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        var applications = await _context.Applications
            .AsNoTracking()
            .Where(application => application.JobSeekerId == jobSeekerId)
            .OrderByDescending(application => application.AppliedAt)
            .ToListAsync(cancellationToken);

        return applications.Select(MapApplication).ToList();
    }

    private static MatchingApplication MapApplication(
        DatabaseApplication application)
    {
        return new MatchingApplication
        {
            Id = application.ApplicationId,
            JobId = application.JobId,
            JobSeekerId = application.JobSeekerId,
            Status = application.Status,
            MatchScoreAtApplication = application.MatchScore,
            AppliedAtUtc = application.AppliedAt
        };
    }
}
