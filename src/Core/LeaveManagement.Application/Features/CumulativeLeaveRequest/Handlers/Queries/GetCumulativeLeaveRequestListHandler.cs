using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.Features.CumulativeLeaveRequest.Requests.Queries;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.CumulativeLeaveRequest.Handlers.Queries;

public class GetCumulativeLeaveRequestListHandler : IRequestHandler<GetCumulativeLeaveRequestListRequest, BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>>
{
    private readonly ICumulativeLeaveRequestService _cumulativeLeaveRequestService;
    private readonly IMapper _mapper;

    public GetCumulativeLeaveRequestListHandler(ICumulativeLeaveRequestService cumulativeLeaveRequestService, IMapper mapper)
    {
        _cumulativeLeaveRequestService=cumulativeLeaveRequestService;
        _mapper=mapper;
    }

    public async Task<BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>> Handle(GetCumulativeLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        if (request.IsManager == false)
        {
            return new BasePaginationResponse<IEnumerable<ListCumulativeLeaveRequestDto>>
            {
                Success = false,
                Message = "You are not manager"
            };
        }
        var cumulativeLeaveRequestList = await _cumulativeLeaveRequestService.GetAll(request.Page, request.Size);

        return cumulativeLeaveRequestList;
        
    }
}