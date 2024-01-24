using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;

public class GetLeaveRequestListRequest : IRequest<BaseResponse<List<LeaveRequestDto>>>
{
    public bool IsManager { get; set; } = true;
}