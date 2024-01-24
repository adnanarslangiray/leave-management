using LeaveManagement.SharedKernel;
using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Domain.Entities;

public class CumulativeLeaveRequest : BaseEntity
{
    public LeaveTypeEnum LeaveType { get; set; }
    public Guid UserId { get; set; }
    public int TotalHours { get; set; }
    public int Year { get; set; }

    public ICollection<Notification> Notifications { get; set; }
}