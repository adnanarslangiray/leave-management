using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.LeaveRequest;

public class UpdateLeaveRequestDto : ILeaveRequestDto
{
    public Guid Id { get; set; }
    public WorkflowStatusEnum WorkflowStatus { get; set; }
}