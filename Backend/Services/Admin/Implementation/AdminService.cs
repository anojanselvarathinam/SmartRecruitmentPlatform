using SmartRecruitmentPlatform.Backend.DTOs.Admin;
using SmartRecruitmentPlatform.Backend.Repositories.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces;
using SmartRecruitmentPlatform.Backend.Models.Authentication;

namespace SmartRecruitmentPlatform.Backend.Services.Admin.Implementation;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        return await _adminRepository.GetDashboardAsync();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _adminRepository.GetUsersAsync();

        return users.Select(user => new UserDto
        {
            Id = user.UserId,
            Name = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive
        }).ToList();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _adminRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.UserId,
            Name = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> UpdateUserStatusAsync(
        int id,
        UpdateUserStatusDto dto)
    {
        return await _adminRepository
            .UpdateUserStatusAsync(id, dto.IsActive);
    }
}