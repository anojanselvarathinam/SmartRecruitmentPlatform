using SmartRecruitmentPlatform.Backend.DTOs.Authentication;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        public Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task<string?> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}