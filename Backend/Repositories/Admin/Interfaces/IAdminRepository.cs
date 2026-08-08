using SmartRecruitmentPlatform.Backend.DTOs.Admin;
using SmartRecruitmentPlatform.Backend.Models.Authentication;


namespace SmartRecruitmentPlatform.Backend.Repositories.Admin.Interfaces;

public interface IAdminRepository
{
    Task<DashboardDto> GetDashboardAsync();

    Task<List<User>> GetUsersAsync();

    Task<User?> GetUserByIdAsync(int id);

    Task<bool> UpdateUserStatusAsync(int id, bool isActive);
}