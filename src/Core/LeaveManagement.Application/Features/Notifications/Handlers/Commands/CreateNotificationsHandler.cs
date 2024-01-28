using AutoMapper;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Repositories;
using MediatR;

namespace LeaveManagement.Application.Features.Notifications.Handlers.Commands;

public class CreateNotificationsHandler : INotificationHandler<NotificationEvent>
{
    private readonly ICumulativeLeaveRequestWriteRepository _cumulativeLeaveRequestWriteRepository;
    private readonly ICumulativeLeaveRequestReadRepository _cumulativeLeaveRequestReadRepository;
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly IMapper _mapper;

    public CreateNotificationsHandler(ICumulativeLeaveRequestWriteRepository cumulativeLeaveRequestWriteRepository, IEmployeeReadRepository employeeReadRepository, ICumulativeLeaveRequestReadRepository cumulativeLeaveRequestReadRepository, IMapper mapper)
    {
        _cumulativeLeaveRequestWriteRepository=cumulativeLeaveRequestWriteRepository;
        _employeeReadRepository=employeeReadRepository;
        _cumulativeLeaveRequestReadRepository=cumulativeLeaveRequestReadRepository;
        _mapper=mapper;
    }

    public async Task Handle(NotificationEvent notification, CancellationToken cancellationToken)
    {
        // notification
        var employee = await _employeeReadRepository.GetByIdAsync(notification.CreateNotificationDto.UserId.ToString());
        var CumulativeLeaveRequest = await _cumulativeLeaveRequestReadRepository.GetByIdAsync(notification.CreateNotificationDto.CumulativeLeaveRequestId.ToString());
        var notificationDto = _mapper.Map<Domain.Entities.Notification>(notification.CreateNotificationDto);
        CumulativeLeaveRequest.Notifications.Add(notificationDto);
        if (employee.ManagerId is not null)
        {
            notificationDto.UserId = (Guid)employee.ManagerId;
            CumulativeLeaveRequest.Notifications.Add(notificationDto);
        }

        _cumulativeLeaveRequestWriteRepository.Update(CumulativeLeaveRequest);
        await _cumulativeLeaveRequestWriteRepository.SaveAsync();
    }
}