using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands; 

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, BaseResponse>
{
    public Task<BaseResponse> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
