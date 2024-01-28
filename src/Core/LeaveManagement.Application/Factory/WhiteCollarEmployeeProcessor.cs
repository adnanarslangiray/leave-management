using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.Factory;

public class WhiteCollarEmployeeProcessor : ILeaveRequestProcessor
{
    public async Task<LeaveRequest> ProcessLeaveRequestAsync(LeaveRequest leaveRequest, ADUser employee)
    {
        leaveRequest.WorkflowStatus = WorkflowStatusEnum.Pending;
        leaveRequest.AssignedUserId = employee.ManagerId;
        return leaveRequest;
    }
}