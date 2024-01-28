using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Repositories;
using LeaveManagement.SharedKernel.Constants;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse>
{
    private readonly ILeaveRequestReadRepository _leaveRequestReadRepository;
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;
    private readonly IMediator _mediatr;

    public UpdateLeaveRequestCommandHandler(ILeaveRequestWriteRepository leaveRequestWriteRepository, ILeaveRequestReadRepository leaveRequestReadRepository, IMediator mediatr)
    {
        _leaveRequestWriteRepository=leaveRequestWriteRepository;
        _leaveRequestReadRepository=leaveRequestReadRepository;
        _mediatr=mediatr;
    }

    public async Task<BaseResponse> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestReadRepository.GetByIdAsync(request.UpdateLeaveRequestDto.Id.ToString());
        if (leaveRequest == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Leave request not found"
            };
        }
        leaveRequest.WorkflowStatus = request.UpdateLeaveRequestDto.WorkflowStatus;
        var updateLeaveRequest = _leaveRequestWriteRepository.Update(leaveRequest);
        await _leaveRequestWriteRepository.SaveAsync();

        //publish update cumulative leave request totalhours
        if (request.UpdateLeaveRequestDto.WorkflowStatus == SharedKernel.Enums.WorkflowStatusEnum.Approved)
        {
            var cumulativeLeaveRequest = new UpdateCumulativeLeaveRequestEvent
            {
                UpdateCumulativeLeaveDto = new DTOs.CumulativeLeave.UpdateCumulativeLeaveDto
                {
                    UserId  = leaveRequest.CreatedById,
                    TotalHours =  StaticVars.CalculateToTalHours(leaveRequest.StartDate, leaveRequest.EndDate),
                    LeaveType = leaveRequest.LeaveType,
                    Year = leaveRequest.StartDate.Year
                }
            };
            await _mediatr.Publish(cumulativeLeaveRequest, cancellationToken);
        }

        return new BaseResponse
        {
            Success = updateLeaveRequest
        };
    }
}