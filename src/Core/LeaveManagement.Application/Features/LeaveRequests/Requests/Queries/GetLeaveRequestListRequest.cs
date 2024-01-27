using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;

public class GetLeaveRequestListRequest : IRequest<BasePaginationResponse<List<LeaveRequestDto>>>
{
    public bool IsManager { get; set; } = true;
    public string EmployeeId { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}