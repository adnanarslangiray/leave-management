using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.LeaveRequest;

public class CreateLeaveRequestDto : ILeaveRequestDto // for mapping profile
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CreatedById { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public string Reason { get; set; }
}