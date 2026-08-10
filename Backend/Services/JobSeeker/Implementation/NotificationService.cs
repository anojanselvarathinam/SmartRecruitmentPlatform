using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;
using SmartRecruitmentPlatform.Backend.Models.JobSeeker;
using SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Implementation;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateApplicationStatusNotificationAsync(
        int jobSeekerProfileId,
        int applicationId,
        string message)
    {
        var notification = new Notification
        {
            JobSeekerProfileId = jobSeekerProfileId,
            ApplicationId = applicationId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<List<NotificationDto>> GetNotificationsAsync(
        int jobSeekerProfileId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(notification =>
                notification.JobSeekerProfileId == jobSeekerProfileId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Select(notification => new NotificationDto
            {
                NotificationId = notification.NotificationId,
                ApplicationId = notification.ApplicationId,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(
        int jobSeekerProfileId,
        int notificationId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(item =>
                item.NotificationId == notificationId &&
                item.JobSeekerProfileId == jobSeekerProfileId);

        if (notification == null)
        {
            return false;
        }

        notification.IsRead = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
