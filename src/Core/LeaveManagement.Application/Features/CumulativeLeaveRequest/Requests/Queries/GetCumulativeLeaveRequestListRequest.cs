using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.CumulativeLeaveRequest.Requests.Queries;

public class GetCumulativeLeaveRequestListRequest : IRequest<BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>>
{
    public bool IsManager { get; set; } = true;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}