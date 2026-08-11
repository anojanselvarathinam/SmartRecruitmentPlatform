using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Repositories.JobSeeker.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class CvService : ICvService
{
    private readonly IJobSeekerRepository _profileRepository;
    private readonly ICvRepository _cvRepository;
    private readonly IWebHostEnvironment _environment;

    public CvService(
        IJobSeekerRepository profileRepository,
        ICvRepository cvRepository,
        IWebHostEnvironment environment)
    {
        _profileRepository = profileRepository;
        _cvRepository = cvRepository;
        _environment = environment;
    }

    public async Task<List<CvDto>> GetCvDocumentsAsync(int userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return new List<CvDto>();
        }

        var cvs = await _cvRepository
            .GetCvDocumentsAsync(profile.Id);

        return cvs.Select(c => new CvDto
        {
            Id = c.Id,
            FileName = c.FileName,
            ContentType = c.ContentType,
            FileSize = c.FileSize,
            UploadedAt = c.UploadedAt
        }).ToList();
    }

    public async Task<CvDto?> UploadCvAsync(
        int userId,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return null;
        }

        var folderPath = Path.Combine(
            _environment.ContentRootPath,
            "CVStorage");

        Directory.CreateDirectory(folderPath);

        var fileName = Guid.NewGuid().ToString()
            + Path.GetExtension(file.FileName);

        var filePath = Path.Combine(
            folderPath,
            fileName);

        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var cv = new CvDocument
        {
            JobSeekerProfileId = profile.Id,
            FileName = file.FileName,
            FilePath = filePath,
            ContentType = file.ContentType,
            FileSize = file.Length,
            UploadedAt = DateTime.UtcNow
        };

        cv = await _cvRepository.AddCvDocumentAsync(cv);

        return new CvDto
        {
            Id = cv.Id,
            FileName = cv.FileName,
            ContentType = cv.ContentType,
            FileSize = cv.FileSize,
            UploadedAt = cv.UploadedAt
        };
    }

    public async Task<bool> DeleteCvAsync(
        int userId,
        int cvId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile == null)
        {
            return false;
        }

        var cv = await _cvRepository
            .GetCvByIdAsync(cvId, profile.Id);

        if (cv == null)
        {
            return false;
        }

        if (File.Exists(cv.FilePath))
        {
            File.Delete(cv.FilePath);
        }

        return await _cvRepository
            .DeleteCvDocumentAsync(cv);
    }
}