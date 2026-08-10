using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Implementation;

public class CvRepository : ICvRepository
{
    private readonly ApplicationDbContext _context;

    public CvRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CvDocument>> GetCvDocumentsAsync(
        int profileId)
    {
        return await _context.Set<CvDocument>()
            .Where(c => c.JobSeekerProfileId == profileId)
            .OrderByDescending(c => c.UploadedAt)
            .ToListAsync();
    }

    public async Task<CvDocument?> GetCvByIdAsync(
        int cvId,
        int profileId)
    {
        return await _context.Set<CvDocument>()
            .FirstOrDefaultAsync(c =>
                c.Id == cvId &&
                c.JobSeekerProfileId == profileId);
    }

    public async Task<CvDocument> AddCvDocumentAsync(
        CvDocument cvDocument)
    {
        await _context.Set<CvDocument>().AddAsync(cvDocument);
        await _context.SaveChangesAsync();

        return cvDocument;
    }

    public async Task<bool> DeleteCvDocumentAsync(
        CvDocument cvDocument)
    {
        _context.Set<CvDocument>().Remove(cvDocument);
        await _context.SaveChangesAsync();

        return true;
    }
}
