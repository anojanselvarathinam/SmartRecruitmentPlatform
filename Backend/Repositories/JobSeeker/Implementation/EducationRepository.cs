using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Implementation;

public class EducationRepository : IEducationRepository
{
    private readonly ApplicationDbContext _context;

    public EducationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Education>> GetEducationAsync(int profileId)
    {
        return await _context.Set<Education>()
            .Where(e => e.JobSeekerProfileId == profileId)
            .ToListAsync();
    }

    public async Task<Education?> GetEducationByIdAsync(
        int educationId,
        int profileId)
    {
        return await _context.Set<Education>()
            .FirstOrDefaultAsync(e =>
                e.Id == educationId &&
                e.JobSeekerProfileId == profileId);
    }

    public async Task<Education> AddEducationAsync(
        Education education)
    {
        await _context.Set<Education>().AddAsync(education);
        await _context.SaveChangesAsync();

        return education;
    }

    public async Task<Education> UpdateEducationAsync(
        Education education)
    {
        _context.Set<Education>().Update(education);
        await _context.SaveChangesAsync();

        return education;
    }

    public async Task<bool> DeleteEducationAsync(
        Education education)
    {
        _context.Set<Education>().Remove(education);
        await _context.SaveChangesAsync();

        return true;
    }
}
