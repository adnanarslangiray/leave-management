using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;
using LeaveManagement.SharedKernel.Utilities;

namespace LeaveManagement.Application.Factory;

public class ManagerProcessor : ILeaveRequestProcessor
{
    public async Task<LeaveRequest> ProcessLeaveRequestAsync(LeaveRequest leaveRequest, ADUser employee)
    {

        leaveRequest.WorkflowStatus = WorkflowStatusEnum.None;
        leaveRequest.AssignedUserId = null;
       return leaveRequest;
    }
}