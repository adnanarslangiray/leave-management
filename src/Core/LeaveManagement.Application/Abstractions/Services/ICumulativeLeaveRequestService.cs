using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Utilities;

namespace LeaveManagement.Application.Abstractions.Services;

public interface ICumulativeLeaveRequestService
{
    Task<PagedList<IEnumerable<CumulativeLeaveRequest>>> GetCumulativeLeaveRequestByUserId(Guid userId, int page, int size);
    Task<IEnumerable<CumulativeLeaveRequest>> GetCurrentYearCumulativeLeaveRequestByUserId(Guid userId);
    Task<CumulativeLeaveRequest> GetCurrentYearCumulativeExcusedLeaveRequestByUserId(Guid userId);
    Task<CumulativeLeaveRequest> GetCurrentYearCumulativeAnnualLeaveRequestByUserId(Guid userId);

    Task<BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>> GetAll(int page, int size);
}