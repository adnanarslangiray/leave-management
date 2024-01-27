using LeaveManagement.Application.DTOs;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Abstractions.Services;

public interface ILeaveRequestService
{
    Task<BaseDataDto<List<LeaveRequest>>> GetLeaveRequestByUserId(string userId, int page, int size);
    Task<List<LeaveRequest>> GetLeaveRequestList(int page, int size);


}