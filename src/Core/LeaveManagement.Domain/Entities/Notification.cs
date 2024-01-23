namespace LeaveManagement.Domain.Entities;

public class Notification
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public Guid CumulativeLeaveRequestId { get; set; }
}