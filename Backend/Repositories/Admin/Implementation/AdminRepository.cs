using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.DTOs.Admin;
//using SmartRecruitmentPlatform.Backend.Models;
using SmartRecruitmentPlatform.Backend.Models.Authentication;
using SmartRecruitmentPlatform.Backend.Repositories.Admin.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Repositories.Admin.Implementation;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var totalUsers = await _context.Users.CountAsync();

        // var totalEmployers = await _context.Employers.CountAsync();

        // var totalJobSeekers = await _context.JobSeekers.CountAsync();

        // var totalVacancies = await _context.Jobs.CountAsync();

        // var totalApplications = await _context.Applications.CountAsync();


        var totalEmployers = 0;
        var totalJobSeekers = 0;
        var totalVacancies = 0;
        var totalApplications = 0;

        return new DashboardDto
        {
            TotalUsers = totalUsers,
            TotalEmployers = totalEmployers,
            TotalJobSeekers = totalJobSeekers,
            TotalVacancies = totalVacancies,
            TotalApplications = totalApplications
        };
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UserId == id);
    }

    public async Task<bool> UpdateUserStatusAsync(
        int id,
        bool isActive)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.UserId == id);

        if (user == null)
        {
            return false;
        }

        user.IsActive = isActive;

        await _context.SaveChangesAsync();

        return true;
    }
}