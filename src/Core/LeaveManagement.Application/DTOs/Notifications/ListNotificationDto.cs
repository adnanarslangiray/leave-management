namespace LeaveManagement.Application.DTOs.Notifications;

public class ListNotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public int Year {get; set; }
    public DateTime CreatedDate { get; set; }
    public string FullName { get; set; }
}