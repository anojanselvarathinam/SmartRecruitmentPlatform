using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Models.Employer;
using SmartRecruitmentPlatform.Backend.Repositories.Employer.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Employer.Implementation
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?> GetByIdAsync(int contactRequestId)
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
            ContactRequest request)
        {
            await _context.ContactRequests.AddAsync(request);

            await _context.SaveChangesAsync();

            return request;
        }

        public async Task UpdateAsync(ContactRequest request)
        {
            _context.ContactRequests.Update(request);

            await _context.SaveChangesAsync();
        }
    }
}