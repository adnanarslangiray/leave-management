using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.CumulativeLeave;

public class CumulativeLeaveCreateDto
{
    public Guid UserId { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public int TotalHours { get; set; }
    public int Year { get; set; }
}