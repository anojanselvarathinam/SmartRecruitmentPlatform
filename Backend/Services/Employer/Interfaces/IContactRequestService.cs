using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IContactRequestService
    {
        Task<ContactRequestResponseDto> SendAsync(
     int employerId,
     SendContactRequestDto dto);

        Task<ContactRequestResponseDto?>
            GetByIdAsync(int contactRequestId, int employerId);

        Task<IEnumerable<ContactRequestResponseDto>>
            GetByEmployerIdAsync(int employerId);

        Task<IEnumerable<ContactRequestResponseDto>>
            GetByJobSeekerIdAsync(int jobSeekerProfileId);

        Task<ContactRequestResponseDto?> UpdateStatusAsync(
            int contactRequestId,
            int jobSeekerProfileId,
            UpdateContactStatusDto dto);
    }
}
