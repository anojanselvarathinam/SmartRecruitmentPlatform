using SmartRecruitmentPlatform.Backend.DTOs.Employer.ContactRequest;
using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Employer.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Employer.Implementations
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository _contactRequestRepository;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository)
        {
            _contactRequestRepository = contactRequestRepository;
        }

        public async Task<ContactRequestResponseDto> SendAsync(
            SendContactRequestDto dto)
        {
            var request = new ContactRequest
            {
                JobSeekerId = dto.JobSeekerId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var created =
                await _contactRequestRepository.CreateAsync(request);

            return MapToResponse(created);
        }

        public async Task<ContactRequestResponseDto?> GetByIdAsync(
            int contactRequestId)
        {
            var request =
                await _contactRequestRepository.GetByIdAsync(
                    contactRequestId);

            if (request == null)
            {
                return null;
            }

            return MapToResponse(request);
        }

        public async Task<IEnumerable<ContactRequestResponseDto>>
            GetByEmployerIdAsync(int employerId)
        {
            var requests =
                await _contactRequestRepository.GetByEmployerIdAsync(
                    employerId);

            return requests.Select(MapToResponse);
        }

        private static ContactRequestResponseDto MapToResponse(
            ContactRequest request)
        {
            return new ContactRequestResponseDto
            {
                ContactRequestId = request.ContactRequestId,
                EmployerId = request.EmployerId,
                JobSeekerId = request.JobSeekerId,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                RespondedAt = request.RespondedAt
            };
        }
    }
}