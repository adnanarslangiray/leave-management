using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Repositories;
using LeaveManagement.SharedKernel.Constants;
using LeaveManagement.SharedKernel.Enums;
using MediatR;

namespace LeaveManagement.Application.Features.CumulativeLeaveRequest.Handlers.Commands;

public class CumulativeLeaveRequestHandler : INotificationHandler<CumulativeLeaveRequestEvent>
{
    private readonly ICumulativeLeaveRequestWriteRepository _cumulativeLeaveRequestWriteRepository;
    private readonly ICumulativeLeaveRequestService _cumulativeLeaveRequestService;
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediatr;

    public CumulativeLeaveRequestHandler(ICumulativeLeaveRequestWriteRepository cumulativeLeaveRequestWriteRepository,
        IMapper mapper,
        ICumulativeLeaveRequestService cumulativeLeaveRequestService,
        ILeaveRequestService leaveRequestService,
        IEmployeeReadRepository employeeReadRepository,
        IMediator mediator)
    {
        _cumulativeLeaveRequestWriteRepository=cumulativeLeaveRequestWriteRepository;
        _mapper=mapper;
        _cumulativeLeaveRequestService=cumulativeLeaveRequestService;
        _leaveRequestService=leaveRequestService;
        _employeeReadRepository=employeeReadRepository;
        _mediatr=mediator;
    }

    public async Task Handle(CumulativeLeaveRequestEvent notification, CancellationToken cancellationToken)
    {
        await AnnualOrExcusedLeaveStatusProcess(notification);

        // update leave request status
        await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(notification.CumulativeLeaveCreateDto.UserId, WorkflowStatusEnum.Approved, notification.CumulativeLeaveCreateDto.LeaveType);

        throw new NotImplementedException();
    }

    private async Task AnnualOrExcusedLeaveStatusProcess(CumulativeLeaveRequestEvent notification)
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));
        // annual = 8x14 = 112
        // excused = 8x5 = 40
        //        AnnualLeave bir sene içerisinde 14 gün olabilir. %10 fazla olduğu durumda exception
        //fırlatılacak ve kullanıcı bilgilendirilecektir. Bu durumda izin talebi Workflow bilgisi her
        //durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
        //• ExcusedAbsence bir sene içerisinde 5 gün olabilir. %20 fazla olduğu durumda exception
        //fırlatılacak ve kullanıcı bilgilendirilecektir. Bu durumda izin talebi Workflow bilgisi her
        //durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
        //• Her izin tipi için müsade edilen gün sayısının %80 i kullanıldığında da kullanıcıya notification
        //yaratılacaktır.

        var CumulativeLeaveRequest = _mapper.Map<Domain.Entities.CumulativeLeaveRequest>(notification.CumulativeLeaveCreateDto);

        if (notification.CumulativeLeaveCreateDto.LeaveType == LeaveTypeEnum.AnnualLeave)
        {
            await AnnualLeaveProcess(notification, CumulativeLeaveRequest);
            //update edilecek
        }
        else if (notification.CumulativeLeaveCreateDto.LeaveType == LeaveTypeEnum.ExcusedAbsence)
        {
            await ExcusedLeaveProcess(notification, CumulativeLeaveRequest);
        }

    }

    private async Task ExcusedLeaveProcess(CumulativeLeaveRequestEvent notification, Domain.Entities.CumulativeLeaveRequest CumulativeLeaveRequest)
    {
        var ExcusedAbsence = await _cumulativeLeaveRequestService.GetCurrentYearCumulativeExcusedLeaveRequestByUserId(notification.CumulativeLeaveCreateDto.UserId);

        int ExcusedAbsenceHours = ExcusedAbsence?.TotalHours ?? 0;
        // eğer daha önce oluşmamışssa
        if (ExcusedAbsence is null)
        {
            CumulativeLeaveRequest.Year = DateTime.UtcNow.Year;
            CumulativeLeaveRequest.TotalHours = 0;
            await _cumulativeLeaveRequestWriteRepository.AddAsync(CumulativeLeaveRequest);
            ExcusedAbsence = CumulativeLeaveRequest;
        }
        int TotalCriticalHours = StaticVars.TotalAnnualLeaveHours - (ExcusedAbsenceHours + notification.CumulativeLeaveCreateDto.TotalHours);
        if ((ExcusedAbsenceHours + notification.CumulativeLeaveCreateDto.TotalHours) >= StaticVars.TotalExcusedLeaveHours * 1.2)
        {
            // burda exception fırlatılacak ve kullanıcı bilgilendirilecektir. Bu durumda izin talebi Workflow bilgisi her durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
            await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(notification.CumulativeLeaveCreateDto.UserId, WorkflowStatusEnum.Exception, LeaveTypeEnum.ExcusedAbsence);


            // notification yaratılacak
            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CumulativeLeaveCreateDto.UserId,
                    Message = $"Aşılan ExcusedAbsence {Math.Abs(TotalCriticalHours)} saat.",
                    CumulativeLeaveRequestId = ExcusedAbsence.Id

                },
                IsSendingManagerNotification = true
            };
            await _mediatr.Publish(notification_mediatr);

            throw new Exception(StaticVars.ExcusedExceptionMessage);
        }
        if ((ExcusedAbsenceHours + notification.CumulativeLeaveCreateDto.TotalHours) >= StaticVars.TotalExcusedLeaveHours * 0.8)
        {

            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CumulativeLeaveCreateDto.UserId,
                    Message = $"Kalan ExcusedAbsence {TotalCriticalHours} saat ",
                    CumulativeLeaveRequestId = ExcusedAbsence.Id

                },
                IsSendingManagerNotification = false
            };
            await _mediatr.Publish(notification_mediatr);

        }
    }

    private async Task AnnualLeaveProcess(CumulativeLeaveRequestEvent notification, Domain.Entities.CumulativeLeaveRequest CumulativeLeaveRequest)
    {
        var AnnualLeave = await _cumulativeLeaveRequestService.GetCurrentYearCumulativeAnnualLeaveRequestByUserId(notification.CumulativeLeaveCreateDto.UserId);

        int AnnualLeaveHours = AnnualLeave?.TotalHours ?? 0;
        if (AnnualLeave is null)
        {
            // eğer daha önce oluşmamışsa
            CumulativeLeaveRequest.TotalHours = 0;
            await _cumulativeLeaveRequestWriteRepository.AddAsync(CumulativeLeaveRequest);

            AnnualLeave = CumulativeLeaveRequest;
        }
        int TotalCriticalHours = StaticVars.TotalAnnualLeaveHours - (AnnualLeaveHours + notification.CumulativeLeaveCreateDto.TotalHours);
        if ((AnnualLeaveHours + notification.CumulativeLeaveCreateDto.TotalHours) >= StaticVars.TotalAnnualLeaveHours * 1.1)
        {
            // update leave request status
            await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(notification.CumulativeLeaveCreateDto.UserId, WorkflowStatusEnum.Exception, LeaveTypeEnum.AnnualLeave);

            // notification yaratılacak
            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CumulativeLeaveCreateDto.UserId,
                    Message = $"Aşılan AnnualLeave {Math.Abs(TotalCriticalHours)} saat.",
                    CumulativeLeaveRequestId = AnnualLeave.Id

                },
                IsSendingManagerNotification = true
            };
            await _mediatr.Publish(notification_mediatr);

            throw new Exception(StaticVars.AnnualExceptionMessage);
        }
        if ((AnnualLeaveHours + notification.CumulativeLeaveCreateDto.TotalHours) >= StaticVars.TotalAnnualLeaveHours * 0.8)
        {
            //notification

            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CumulativeLeaveCreateDto.UserId,
                    Message = $"Kalan AnnualLeave {TotalCriticalHours} saat ",
                    CumulativeLeaveRequestId = AnnualLeave.Id

                },
                IsSendingManagerNotification = false
            };
            await _mediatr.Publish(notification_mediatr);
        }
    }
}