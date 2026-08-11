using Microsoft.AspNetCore.Http;
using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface ICvService
{
    Task<List<CvDto>> GetCvDocumentsAsync(int userId);

    Task<CvDto?> UploadCvAsync(
        int userId,
        IFormFile file);

    Task<bool> DeleteCvAsync(
        int userId,
        int cvId);
}