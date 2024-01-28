using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Utilities;

namespace LeaveManagement.Application.Factory;

public interface ILeaveRequestProcessor
{
    Task<LeaveRequest> ProcessLeaveRequestAsync(LeaveRequest leaveRequest, ADUser employee);
}