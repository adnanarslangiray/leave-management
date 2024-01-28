using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Abstractions.Services;

public interface ICumulativeLeaveRequestService
{
    IEnumerable<CumulativeLeaveRequest> GetCumulativeLeaveRequestByUserId(Guid userId);
    IEnumerable<LeaveManagement.Domain.Entities.CumulativeLeaveRequest> GetCurrentYearCumulativeLeaveRequestByUserId(Guid userId);
    Task<CumulativeLeaveRequest> GetCurrentYearCumulativeExcusedLeaveRequestByUserId(Guid userId);
    Task<CumulativeLeaveRequest> GetCurrentYearCumulativeAnnualLeaveRequestByUserId(Guid userId);
}