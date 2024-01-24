using AutoMapper;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Enums;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseResponse>
{
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;
    private readonly IEmployeeReadRepository _userReadRepository;
    private readonly IMapper _mapper;

    public CreateLeaveRequestCommandHandler(ILeaveRequestWriteRepository leaveRequestWriteRepository, IMapper mapper, IEmployeeReadRepository userReadRepository)
    {
        _leaveRequestWriteRepository=leaveRequestWriteRepository;
        _mapper=mapper;
        _userReadRepository=userReadRepository;
    }


    public async Task<BaseResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = _mapper.Map<LeaveRequest>(request);

        var employee = await _userReadRepository.GetByIdAsync(leaveRequest.CreatedById.ToString());
        if (employee == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Employee not found"
            };
        }

        #region Factory Pattern Uygulanacak!

        // burada factory pattern kullanılacak
        if (employee.UserType == UserType.WhiteCollarEmployee)
        {
            leaveRequest.WorkflowStatus = WorkflowStatusEnum.Pending;
            leaveRequest.AssignedUserId = employee.ManagerId;
        }
        else if (employee.UserType == UserType.BlueCollarEmployee)
        {
            if (leaveRequest.LeaveType == LeaveTypeEnum.AnnualLeave)
            {
                leaveRequest.WorkflowStatus = WorkflowStatusEnum.Pending;
                var manager = await _userReadRepository.GetByIdAsync(employee.ManagerId.ToString());
                if (manager == null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Manager not found"
                    };
                }
                leaveRequest.AssignedUserId = manager.ManagerId;
            }
            else if (leaveRequest.LeaveType == LeaveTypeEnum.ExcusedAbsence)
            {
                leaveRequest.WorkflowStatus = WorkflowStatusEnum.Pending;
                leaveRequest.AssignedUserId = employee.ManagerId;
            }
        }
        else if (employee.UserType == UserType.Manager)
        {
            leaveRequest.WorkflowStatus = WorkflowStatusEnum.None;
            leaveRequest.AssignedUserId = null;
        }

        #endregion Factory Pattern Uygulanacak!

        leaveRequest.LastModifiedById = employee.Id;
        leaveRequest.RequestNumber = DateTime.Now.Ticks.ToString();

        var result = await _leaveRequestWriteRepository.AddAsync(leaveRequest);
        await _leaveRequestWriteRepository.SaveAsync();

        return new BaseResponse
        {
            Success = result,
            Id = result ? leaveRequest.Id.ToString() : default,
            Message =  result ? "Leave Request added" : "Failed"
        };
    }
}