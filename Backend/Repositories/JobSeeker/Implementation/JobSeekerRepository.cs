using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Implementation;

public class JobSeekerRepository : IJobSeekerRepository
{
    private readonly ApplicationDbContext _context;

    public JobSeekerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobSeekerProfile?> GetProfileAsync(int userId)
    {
        return await _context.Set<JobSeekerProfile>()
            .Include(p => p.Skills)
            .Include(p => p.Educations)
            .Include(p => p.Experiences)
            .Include(p => p.CvDocuments)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<JobSeekerProfile?> GetProfileByIdAsync(int profileId)
    {
        return await _context.Set<JobSeekerProfile>()
            .Include(p => p.Skills)
            .Include(p => p.Educations)
            .Include(p => p.Experiences)
            .Include(p => p.CvDocuments)
            .FirstOrDefaultAsync(p => p.Id == profileId);
    }

    public async Task<JobSeekerProfile> CreateProfileAsync(
        JobSeekerProfile profile)
    {
        await _context.Set<JobSeekerProfile>().AddAsync(profile);
        await _context.SaveChangesAsync();

        return profile;
    }

    public async Task<JobSeekerProfile> UpdateProfileAsync(
        JobSeekerProfile profile)
    {
        _context.Set<JobSeekerProfile>().Update(profile);
        await _context.SaveChangesAsync();

        return profile;
    }
}