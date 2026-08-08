using SmartRecruitmentPlatform.Backend.DTOs.Admin;

namespace SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces;

public interface IAdminService
{
    Task<DashboardDto> GetDashboardAsync();

    Task<List<UserDto>> GetUsersAsync();

    Task<UserDto?> GetUserByIdAsync(int id);

    Task<bool> UpdateUserStatusAsync(
        int id,
        UpdateUserStatusDto dto);
}