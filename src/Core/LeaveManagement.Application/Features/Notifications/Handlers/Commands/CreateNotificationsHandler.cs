using AutoMapper;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Repositories;
using MediatR;

namespace LeaveManagement.Application.Features.Notifications.Handlers.Commands;

public class CreateNotificationsHandler : INotificationHandler<NotificationEvent>
{
    private readonly INotificationWriteRepository _notificationWriteRepository;
    private readonly ICumulativeLeaveRequestReadRepository _cumulativeLeaveRequestReadRepository;
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly IMapper _mapper;

    public CreateNotificationsHandler(INotificationWriteRepository notificationWriteRepository, IEmployeeReadRepository employeeReadRepository, ICumulativeLeaveRequestReadRepository cumulativeLeaveRequestReadRepository, IMapper mapper)
    {
         _notificationWriteRepository = notificationWriteRepository;
        _employeeReadRepository=employeeReadRepository;
        _cumulativeLeaveRequestReadRepository=cumulativeLeaveRequestReadRepository;
        _mapper=mapper;
    }

    public async Task Handle(NotificationEvent notification, CancellationToken cancellationToken)
    {
        // notification
        var employee = await _employeeReadRepository.GetByIdAsync(notification.CreateNotificationDto.UserId.ToString());
        var notificationDto = _mapper.Map<Domain.Entities.Notification>(notification.CreateNotificationDto);
        notificationDto.CreatedAt = DateTime.UtcNow;
        notificationDto.Year = DateTime.UtcNow.Year;
        List<Domain.Entities.Notification> listNotification = [notificationDto];
        if (employee.ManagerId is not null)
        {
            var notificationManagerDto = _mapper.Map<Domain.Entities.Notification>(notification.CreateNotificationDto);
            notificationManagerDto.CreatedAt = DateTime.UtcNow;
            notificationManagerDto.Year = DateTime.UtcNow.Year;
            notificationManagerDto.UserId = (Guid)employee.ManagerId;
            notificationManagerDto.Id = Guid.NewGuid();
            listNotification.Add(notificationManagerDto);
        }
        await _notificationWriteRepository.AddRangeAsync(listNotification);
        await _notificationWriteRepository.SaveAsync();
    }
}