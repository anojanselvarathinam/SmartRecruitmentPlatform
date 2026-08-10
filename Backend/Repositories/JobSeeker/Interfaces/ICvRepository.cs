using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;

public interface ICvRepository
{
    Task<List<CvDocument>> GetCvDocumentsAsync(int profileId);

    Task<CvDocument?> GetCvByIdAsync(int cvId, int profileId);

    Task<CvDocument> AddCvDocumentAsync(CvDocument cvDocument);

    Task<bool> DeleteCvDocumentAsync(CvDocument cvDocument);
}
namespace SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces
{
    public interface ICvRepository
    {
    }
}