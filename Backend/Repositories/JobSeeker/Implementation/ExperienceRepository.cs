using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Implementation;

public class ExperienceRepository : IExperienceRepository
{
    private readonly ApplicationDbContext _context;

    public ExperienceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Experience>> GetExperiencesAsync(int profileId)
    {
        return await _context.Set<Experience>()
            .Where(e => e.JobSeekerProfileId == profileId)
            .ToListAsync();
    }

    public async Task<Experience?> GetExperienceByIdAsync(
        int experienceId,
        int profileId)
    {
        return await _context.Set<Experience>()
            .FirstOrDefaultAsync(e =>
                e.Id == experienceId &&
                e.JobSeekerProfileId == profileId);
    }

    public async Task<Experience> AddExperienceAsync(
        Experience experience)
    {
        await _context.Set<Experience>().AddAsync(experience);
        await _context.SaveChangesAsync();

        return experience;
    }

    public async Task<Experience> UpdateExperienceAsync(
        Experience experience)
    {
        _context.Set<Experience>().Update(experience);
        await _context.SaveChangesAsync();

        return experience;
    }

    public async Task<bool> DeleteExperienceAsync(
        Experience experience)
    {
        _context.Set<Experience>().Remove(experience);
        await _context.SaveChangesAsync();

        return true;
    }
}
