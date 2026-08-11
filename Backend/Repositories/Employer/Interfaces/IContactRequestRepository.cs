using SmartRecruitmentPlatform.Backend.Models;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IContactRequestRepository
    {
        Task<ContactRequest?> GetByIdAsync(int contactRequestId);

        Task<IEnumerable<ContactRequest>> GetByEmployerIdAsync(
            int employerId);

        Task<IEnumerable<ContactRequest>> GetByJobSeekerIdAsync(
            int jobSeekerId);

        Task<ContactRequest> CreateAsync(
            ContactRequest contactRequest);

        Task UpdateAsync(
            ContactRequest contactRequest);
    }
}