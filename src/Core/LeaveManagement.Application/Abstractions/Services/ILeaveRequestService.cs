using LeaveManagement.Application.DTOs;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.Abstractions.Services;

public interface ILeaveRequestService
{
    Task<BaseDataDto<List<LeaveRequest>>> GetLeaveRequestByUserId(string userId, int page, int size);
    Task<List<LeaveRequest>> GetLeaveRequestList(int page, int size);
    Task<bool> UpdateLeaveRequestWorkflowStatusByUserId(Guid UserId, WorkflowStatusEnum workflowStatus, LeaveTypeEnum leaveType);


}