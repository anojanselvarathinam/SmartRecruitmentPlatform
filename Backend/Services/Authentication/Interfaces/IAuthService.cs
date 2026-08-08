using SmartRecruitmentPlatform.Backend.DTOs.Authentication;

namespace SmartRecruitmentPlatform.Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);

        Task<string?> LoginAsync(LoginDto loginDto);
    }
}