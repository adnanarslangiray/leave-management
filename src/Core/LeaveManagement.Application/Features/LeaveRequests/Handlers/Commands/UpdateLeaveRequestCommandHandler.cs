using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse>
{
    private readonly ILeaveRequestService _leaveRequestService;

    public UpdateLeaveRequestCommandHandler(ILeaveRequestService leaveRequestService)
    {
        _leaveRequestService=leaveRequestService;
    }

    public async Task<BaseResponse> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(request.UpdateLeaveRequestDto.Id, request.UpdateLeaveRequestDto.WorkflowStatus);

        return new BaseResponse
        {
            Success = result
        };
    }
}