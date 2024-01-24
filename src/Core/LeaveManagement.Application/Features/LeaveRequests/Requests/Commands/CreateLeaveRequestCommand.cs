using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;

public class CreateLeaveRequestCommand : IRequest<BaseResponse>
{
    public CreateLeaveRequestDto LeaveRequestDto { get; set; }
}
