using LeaveManagement.SharedKernel.Enum;

namespace LeaveManagement.Domain.Entities;

public class CumulativeLeaveRequest
{
    public LeaveTypeEnum LeaveType { get; set; }
    public Guid UserId { get; set; }
    public int TotalHours { get; set; }
    public int Year { get; set; }
}