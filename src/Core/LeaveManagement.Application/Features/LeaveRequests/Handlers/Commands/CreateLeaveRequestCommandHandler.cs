using AutoMapper;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Factory;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Constants;
using LeaveManagement.SharedKernel.Enums;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseResponse>
{
    private readonly ILeaveRequestWriteRepository _leaveRequestWriteRepository;
    private readonly IEmployeeReadRepository _userReadRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediatr;
    private readonly IServiceProvider _serviceProvider;

    public CreateLeaveRequestCommandHandler(ILeaveRequestWriteRepository leaveRequestWriteRepository, IMapper mapper, IEmployeeReadRepository userReadRepository, IMediator mediatr, IServiceProvider serviceProvider)
    {
        _leaveRequestWriteRepository=leaveRequestWriteRepository;
        _mapper=mapper;
        _userReadRepository=userReadRepository;
        _mediatr=mediatr;
        _serviceProvider=serviceProvider;
    }


    public async Task<BaseResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = _mapper.Map<LeaveRequest>(request.LeaveRequestDto);

        var employee = await _userReadRepository.GetByIdAsync(leaveRequest.CreatedById.ToString());
        if (employee == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Employee not found"
            };
        }

        #region Factory Pattern Uygulandı!
        var processor = _serviceProvider.GetServices<ILeaveRequestProcessor>()
           .FirstOrDefault(p => p.GetType() == GetProcessorType(employee.UserType));

        leaveRequest = await processor.ProcessLeaveRequestAsync(leaveRequest, employee);

        #endregion Factory Pattern Uygulanacak!

        leaveRequest.LastModifiedById = employee.Id;
        leaveRequest.RequestNumber = DateTime.Now.Ticks.ToString();
        leaveRequest.FormNumber = DateTime.Now.Ticks.ToString();

        var result = await _leaveRequestWriteRepository.AddAsync(leaveRequest);
        await _leaveRequestWriteRepository.SaveAsync();

        // event publish edilecek
        var cumulateLeaveRequestEvent = new CreateCumulativeLeaveRequestEvent()
        {
            CreateCumulativeLeaveDto = new CreateCumulativeLeaveDto()
            {
                LeaveType = leaveRequest.LeaveType,
                TotalHours =StaticVars.CalculateToTalHours(leaveRequest.StartDate, leaveRequest.EndDate),
                UserId = leaveRequest.CreatedById,
                Year = leaveRequest.StartDate.Year

            }

        };
        await _mediatr.Publish(cumulateLeaveRequestEvent, cancellationToken);

        return new BaseResponse
        {
            Success = result,
            Id = result ? leaveRequest.Id.ToString() : default,
            Message =  result ? "Created" : "Failed"
        };
    }

    private static Type GetProcessorType(UserType userType)
    {
        return userType switch
        {
            UserType.Manager => typeof(ManagerProcessor),
            UserType.WhiteCollarEmployee => typeof(WhiteCollarEmployeeProcessor),
            UserType.BlueCollarEmployee => typeof(BlueCollarEmployeeProcessor),
            _ => typeof(ManagerProcessor)
        };
    }
}