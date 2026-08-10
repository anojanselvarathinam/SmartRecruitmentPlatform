using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces
{
    public interface IContactRequestService
    {
        Task<ContactRequestResponseDto> SendAsync(
            SendContactRequestDto dto);

        Task<ContactRequestResponseDto?> GetByIdAsync(
            int contactRequestId);

        Task<IEnumerable<ContactRequestResponseDto>>
            GetByEmployerIdAsync(int employerId);
    }
}