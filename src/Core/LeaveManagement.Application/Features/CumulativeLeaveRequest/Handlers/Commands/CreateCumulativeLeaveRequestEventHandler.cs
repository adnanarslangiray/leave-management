using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Application.Events.DomainEvents.Concretes;
using LeaveManagement.Application.Repositories;
using LeaveManagement.SharedKernel.Constants;
using LeaveManagement.SharedKernel.Enums;
using MediatR;

namespace LeaveManagement.Application.Features.CumulativeLeaveRequest.Handlers.Commands;

public class CreateCumulativeLeaveRequestEventHandler : INotificationHandler<CreateCumulativeLeaveRequestEvent>
{
    private readonly ICumulativeLeaveRequestWriteRepository _cumulativeLeaveRequestWriteRepository;
    private readonly ICumulativeLeaveRequestService _cumulativeLeaveRequestService;
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediatr;

    public CreateCumulativeLeaveRequestEventHandler(ICumulativeLeaveRequestWriteRepository cumulativeLeaveRequestWriteRepository,
        IMapper mapper,
        ICumulativeLeaveRequestService cumulativeLeaveRequestService,
        ILeaveRequestService leaveRequestService,
        IMediator mediator)
    {
        _cumulativeLeaveRequestWriteRepository=cumulativeLeaveRequestWriteRepository;
        _mapper=mapper;
        _cumulativeLeaveRequestService=cumulativeLeaveRequestService;
        _leaveRequestService=leaveRequestService;
        _mediatr=mediator;
    }

    public async Task Handle(CreateCumulativeLeaveRequestEvent notification, CancellationToken cancellationToken)
    {
        await AnnualOrExcusedLeaveStatusProcess(notification, cancellationToken);


    }

    private async Task AnnualOrExcusedLeaveStatusProcess(CreateCumulativeLeaveRequestEvent notification, CancellationToken cancellationToken)
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

        var cumulativeLeaveRequest = _mapper.Map<Domain.Entities.CumulativeLeaveRequest>(notification.CreateCumulativeLeaveDto);

        if (notification.CreateCumulativeLeaveDto.LeaveType == LeaveTypeEnum.AnnualLeave)
        {
            await AnnualLeaveProcess(notification, cumulativeLeaveRequest, cancellationToken);
            return;
        }

        await ExcusedLeaveProcess(notification, cumulativeLeaveRequest, cancellationToken);


    }

    private async Task ExcusedLeaveProcess(CreateCumulativeLeaveRequestEvent notification, Domain.Entities.CumulativeLeaveRequest cumulativeLeaveRequest, CancellationToken cancellationToken)
    {
        var excusedAbsence = await _cumulativeLeaveRequestService.GetCurrentYearCumulativeExcusedLeaveRequestByUserId(notification.CreateCumulativeLeaveDto.UserId);

        int excusedAbsenceHours = excusedAbsence?.TotalHours ?? 0;
        // eğer daha önce oluşmamışssa
        if (excusedAbsence is null)
        {
            cumulativeLeaveRequest.Year = DateTime.UtcNow.Year;
            cumulativeLeaveRequest.TotalHours = 0;
            await _cumulativeLeaveRequestWriteRepository.AddAsync(cumulativeLeaveRequest);
            await _cumulativeLeaveRequestWriteRepository.SaveAsync();
            excusedAbsence = cumulativeLeaveRequest;
        }
        int totalCriticalHours = StaticVars.TotalAnnualLeaveHours - (excusedAbsenceHours + notification.CreateCumulativeLeaveDto.TotalHours);
        if ((excusedAbsenceHours + notification.CreateCumulativeLeaveDto.TotalHours) >= StaticVars.TotalExcusedLeaveHours * 1.2)
        {
            // burda exception fırlatılacak ve kullanıcı bilgilendirilecektir. Bu durumda izin talebi Workflow bilgisi her durumda Exception olacak ve kullanıcı ile varsa Manager’a bildirim yapılacaktır.
            await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(notification.CreateCumulativeLeaveDto.UserId, WorkflowStatusEnum.Exception, LeaveTypeEnum.ExcusedAbsence);


            // notification yaratılacak
            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CreateCumulativeLeaveDto.UserId,
                    Message = $"Aşılan ExcusedAbsence {Math.Abs(totalCriticalHours)} saat.",
                    CumulativeLeaveRequestId = excusedAbsence.Id

                },
                IsSendingManagerNotification = true
            };
            await _mediatr.Publish(notification_mediatr, cancellationToken);

            throw new Exception(StaticVars.ExcusedExceptionMessage);
        }
        if ((excusedAbsenceHours + notification.CreateCumulativeLeaveDto.TotalHours) >= StaticVars.TotalExcusedLeaveHours * 0.8)
        {

            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CreateCumulativeLeaveDto.UserId,
                    Message = $"Kalan ExcusedAbsence {totalCriticalHours} saat ",
                    CumulativeLeaveRequestId = excusedAbsence.Id

                },
                IsSendingManagerNotification = false
            };
            await _mediatr.Publish(notification_mediatr, cancellationToken);

        }
    }

    private async Task AnnualLeaveProcess(CreateCumulativeLeaveRequestEvent notification, Domain.Entities.CumulativeLeaveRequest cumulativeLeaveRequest, CancellationToken cancellationToken)
    {
        var annualLeave = await _cumulativeLeaveRequestService.GetCurrentYearCumulativeAnnualLeaveRequestByUserId(notification.CreateCumulativeLeaveDto.UserId);

        int annualLeaveHours = annualLeave?.TotalHours ?? 0;
        if (annualLeave is null)
        {
            // eğer daha önce oluşmamışsa
            cumulativeLeaveRequest.TotalHours = 0;
            await _cumulativeLeaveRequestWriteRepository.AddAsync(cumulativeLeaveRequest);
            await _cumulativeLeaveRequestWriteRepository.SaveAsync();

            annualLeave = cumulativeLeaveRequest;
        }
        int totalCriticalHours = StaticVars.TotalAnnualLeaveHours - (annualLeaveHours + notification.CreateCumulativeLeaveDto.TotalHours);
        if ((annualLeaveHours + notification.CreateCumulativeLeaveDto.TotalHours) >= StaticVars.TotalAnnualLeaveHours * 1.1)
        {
            // update leave request status
            await _leaveRequestService.UpdateLeaveRequestWorkflowStatusByUserId(notification.CreateCumulativeLeaveDto.UserId, WorkflowStatusEnum.Exception, LeaveTypeEnum.AnnualLeave);

            // notification yaratılacak
            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CreateCumulativeLeaveDto.UserId,
                    Message = $"Aşılan AnnualLeave {Math.Abs(totalCriticalHours)} saat.",
                    CumulativeLeaveRequestId = annualLeave.Id

                },
                IsSendingManagerNotification = true
            };
            await _mediatr.Publish(notification_mediatr, cancellationToken);

            throw new Exception(StaticVars.AnnualExceptionMessage);
        }
        if ((annualLeaveHours + notification.CreateCumulativeLeaveDto.TotalHours) >= StaticVars.TotalAnnualLeaveHours * 0.8)
        {
            //notification

            var notification_mediatr = new NotificationEvent()
            {
                CreateNotificationDto = new CreateNotificationDto()
                {
                    UserId = notification.CreateCumulativeLeaveDto.UserId,
                    Message = $"Kalan AnnualLeave {totalCriticalHours} saat ",
                    CumulativeLeaveRequestId = annualLeave.Id

                },
                IsSendingManagerNotification = false
            };
            await _mediatr.Publish(notification_mediatr, cancellationToken);
        }
    }
}