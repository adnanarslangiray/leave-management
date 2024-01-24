using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Repositories;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse>
{
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;
    public Task<BaseResponse> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}