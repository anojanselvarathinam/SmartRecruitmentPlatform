using SmartRecruitmentPlatform.Backend.Models.Authentication;

namespace SmartRecruitmentPlatform.Backend.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task AddUserAsync(User user);

        Task SaveChangesAsync();
    }
}