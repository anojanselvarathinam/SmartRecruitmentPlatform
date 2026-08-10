using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using MatchingProfile = SmartRecruitmentPlatform.Backend.Models.JobMatching.JobSeekerProfile;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobMatching;

public class EfJobSeekerProfileRepository : IJobSeekerProfileRepository
{
    private readonly ApplicationDbContext _context;

    public EfJobSeekerProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MatchingProfile?> GetByJobSeekerIdAsync(
        int jobSeekerId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _context.JobSeekerProfiles
            .AsNoTracking()
            .Include(item => item.Skills)
            .Include(item => item.Educations)
            .Include(item => item.Experiences)
            .FirstOrDefaultAsync(
                item => item.Id == jobSeekerId,
                cancellationToken);

        if (profile == null)
        {
            return null;
        }

        return new MatchingProfile
        {
            JobSeekerId = profile.Id,
            FullName = $"{profile.FirstName} {profile.LastName}".Trim(),
            Location = profile.Location ?? string.Empty,
            ExperienceYears = CalculateExperienceYears(
                profile.Experiences,
                profile.UpdatedAt ?? profile.CreatedAt),
            Education = string.Join(
                ", ",
                profile.Educations.Select(education => education.Degree)),
            Skills = profile.Skills
                .Select(skill => skill.SkillName)
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList()
        };
    }

    private static decimal CalculateExperienceYears(
        IEnumerable<Models.JobSeeker.Experience> experiences,
        DateTime profileDate)
    {
        var totalDays = 0d;
        var currentExperienceEndDate = profileDate.Date;

        foreach (var experience in experiences)
        {
            var endDate = experience.EndDate?.Date ?? currentExperienceEndDate;

            if (endDate > experience.StartDate.Date)
            {
                totalDays += (endDate - experience.StartDate.Date).TotalDays;
            }
        }

        return Math.Round((decimal)(totalDays / 365.25d), 2);
    }
}
