using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.LeaveRequest;

public class UpdateLeaveRequestDto : ILeaveRequestDto
{
    public Guid CreatedById { get; set; }
    public Guid Id { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public WorkflowStatusEnum WorkflowStatus { get; set; }
}