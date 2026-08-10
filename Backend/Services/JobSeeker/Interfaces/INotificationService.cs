using SmartRecruitmentPlatform.Backend.DTOs.JobSeeker;

namespace SmartRecruitmentPlatform.Backend.Services.JobSeeker.Interfaces;

public interface INotificationService
{
    Task CreateApplicationStatusNotificationAsync(
        int jobSeekerProfileId,
        int applicationId,
        string message);

    Task<List<NotificationDto>> GetNotificationsAsync(int jobSeekerProfileId);

    Task<bool> MarkAsReadAsync(int jobSeekerProfileId, int notificationId);
}
