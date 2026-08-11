using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Implementations
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?> GetByIdAsync(
            int contactRequestId)
        {
            return await _context.ContactRequests
                .FirstOrDefaultAsync(c =>
                    c.ContactRequestId == contactRequestId);
        }

        public async Task<IEnumerable<ContactRequest>> GetByEmployerIdAsync(
            int employerId)
        {
            return await _context.ContactRequests
                .Where(c => c.EmployerId == employerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContactRequest>> GetByJobSeekerIdAsync(
            int jobSeekerId)
        {
            return await _context.ContactRequests
                .Where(c => c.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<ContactRequest> CreateAsync(
            ContactRequest contactRequest)
        {
            _context.ContactRequests.Add(contactRequest);

            await _context.SaveChangesAsync();

            return contactRequest;
        }

        public async Task UpdateAsync(
            ContactRequest contactRequest)
        {
            _context.ContactRequests.Update(contactRequest);

            await _context.SaveChangesAsync();
        }
    }
}