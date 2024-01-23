using LeaveManagement.SharedKernel;

namespace LeaveManagement.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public Guid CumulativeLeaveRequestId { get; set; }
}