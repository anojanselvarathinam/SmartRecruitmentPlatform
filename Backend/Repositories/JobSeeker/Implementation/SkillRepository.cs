using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Implementation;

public class SkillRepository : ISkillRepository
{
    private readonly ApplicationDbContext _context;

    public SkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobSeekerSkill>> GetSkillsAsync(int profileId)
    {
        return await _context.Set<JobSeekerSkill>()
            .Where(s => s.JobSeekerProfileId == profileId)
            .ToListAsync();
    }

    public async Task<JobSeekerSkill?> GetSkillByIdAsync(
        int skillId,
        int profileId)
    {
        return await _context.Set<JobSeekerSkill>()
            .FirstOrDefaultAsync(s =>
                s.Id == skillId &&
                s.JobSeekerProfileId == profileId);
    }

    public async Task<JobSeekerSkill> AddSkillAsync(
        JobSeekerSkill skill)
    {
        await _context.Set<JobSeekerSkill>().AddAsync(skill);
        await _context.SaveChangesAsync();

        return skill;
    }

    public async Task<JobSeekerSkill> UpdateSkillAsync(
        JobSeekerSkill skill)
    {
        _context.Set<JobSeekerSkill>().Update(skill);
        await _context.SaveChangesAsync();

        return skill;
    }

    public async Task<bool> DeleteSkillAsync(
        JobSeekerSkill skill)
    {
        _context.Set<JobSeekerSkill>().Remove(skill);
        await _context.SaveChangesAsync();

        return true;
    }
}