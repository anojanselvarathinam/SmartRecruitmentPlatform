using SmartRecruitmentPlatform.Backend.Models.Authentication;

using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task RegisterUserAsync(
            User user,
            Employer? employer,
            JobSeekerProfile? jobSeekerProfile);
    }
}
