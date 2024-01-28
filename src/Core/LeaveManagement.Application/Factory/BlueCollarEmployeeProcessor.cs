using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.Factory;

public class BlueCollarEmployeeProcessor : ILeaveRequestProcessor
{
    private readonly IEmployeeReadRepository _userReadRepository;

    public BlueCollarEmployeeProcessor(IEmployeeReadRepository userReadRepository)
    {
        _userReadRepository=userReadRepository;
    }

    public async Task<LeaveRequest> ProcessLeaveRequestAsync(LeaveRequest leaveRequest, ADUser employee)
    {
        if (leaveRequest.LeaveType == LeaveTypeEnum.AnnualLeave)
        {
            var manager = await _userReadRepository.GetByIdAsync(employee.ManagerId.ToString());
            leaveRequest.AssignedUserId = manager?.ManagerId;
        }
        else
            leaveRequest.AssignedUserId = employee.ManagerId;

        leaveRequest.WorkflowStatus = WorkflowStatusEnum.Pending;
        return leaveRequest;
    }
}