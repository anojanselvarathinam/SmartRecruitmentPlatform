using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.DTOs.Admin;
using SmartRecruitmentPlatform.Backend.Services.Admin.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrator")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _adminService.GetDashboardAsync();

        return Ok(dashboard);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();

        return Ok(users);
    }

    [HttpGet("users/{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _adminService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        return Ok(user);
    }

    [HttpPut("users/{id:int}/status")]
    public async Task<IActionResult> UpdateUserStatus(
        int id,
        [FromBody] UpdateUserStatusDto dto)
    {
        var updated = await _adminService
            .UpdateUserStatusAsync(id, dto);

        if (!updated)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        return Ok(new
        {
            message = "User status updated successfully."
        });
    }
}