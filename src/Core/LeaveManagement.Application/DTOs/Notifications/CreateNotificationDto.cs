using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.DTOs.Notifications;

public class CreateNotificationDto
{
    public string Message { get; set; }
    public Guid UserId { get; set; }
    public Guid CumulativeLeaveRequestId { get; set; }
}