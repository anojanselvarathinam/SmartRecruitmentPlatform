using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Controllers.JobSeeker;

[ApiController]
[Route("api/jobseeker/notifications")]
[Authorize(Roles = "JobSeeker")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        if (!TryGetProfileId(out int profileId))
            return Unauthorized("Job seeker profile ID not found in token.");

        var notifications = await _notificationService
            .GetNotificationsAsync(profileId);

        return Ok(notifications);
    }

    [HttpPut("{notificationId:int}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        if (!TryGetProfileId(out int profileId))
            return Unauthorized("Job seeker profile ID not found in token.");

        var updated = await _notificationService.MarkAsReadAsync(
            profileId,
            notificationId);

        if (!updated)
            return NotFound("Notification not found.");

        return Ok(new { message = "Notification marked as read." });
    }

    private bool TryGetProfileId(out int profileId)
    {
        var profileIdClaim = User.FindFirst("jobSeekerProfileId")?.Value;
        return int.TryParse(profileIdClaim, out profileId);
    }
}
