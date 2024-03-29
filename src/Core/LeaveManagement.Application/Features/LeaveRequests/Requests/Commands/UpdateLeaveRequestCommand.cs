﻿using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;

public class UpdateLeaveRequestCommand : IRequest<BaseResponse>
{
    public UpdateLeaveRequestDto UpdateLeaveRequestDto { get; set; }

}