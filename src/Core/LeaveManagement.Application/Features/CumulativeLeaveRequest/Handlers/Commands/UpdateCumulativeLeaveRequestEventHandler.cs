using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Repositories;
using MediatR;

namespace LeaveManagement.Application.Features.CumulativeLeaveRequest.Handlers.Commands;

public class UpdateCumulativeLeaveRequestEventHandler : INotificationHandler<UpdateCumulativeLeaveRequestEvent>
{

    private readonly ICumulativeLeaveRequestWriteRepository _cumulativeLeaveRequestWriteRepository;
    private readonly ICumulativeLeaveRequestService _cumulativeLeaveRequestService;
    private readonly IMapper _mapper;

    public UpdateCumulativeLeaveRequestEventHandler(IMapper mapper, ICumulativeLeaveRequestService cumulativeLeaveRequestService, ICumulativeLeaveRequestWriteRepository cumulativeLeaveRequestWriteRepository)
    {
        _mapper=mapper;
        _cumulativeLeaveRequestService=cumulativeLeaveRequestService;
        _cumulativeLeaveRequestWriteRepository=cumulativeLeaveRequestWriteRepository;
    }

    public async Task Handle(UpdateCumulativeLeaveRequestEvent notification, CancellationToken cancellationToken)
    {
      
            var getCumulativeLeaveRequest = await _cumulativeLeaveRequestService.GetCurrentYearCumulativeLeaveRequestByUserId(notification.UpdateCumulativeLeaveDto.UserId);
            var cumulativeLeaveRequest = getCumulativeLeaveRequest.FirstOrDefault(x=>x.LeaveType == notification.UpdateCumulativeLeaveDto.LeaveType);

            cumulativeLeaveRequest.TotalHours += notification.UpdateCumulativeLeaveDto.TotalHours;
            _cumulativeLeaveRequestWriteRepository.Update(cumulativeLeaveRequest);
            await _cumulativeLeaveRequestWriteRepository.SaveAsync();
    
    }
}