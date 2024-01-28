using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Application.Features.Notifications.Requests.Queries;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.Notifications.Handlers.Queries;

public class GetNotificationListHandler : IRequestHandler<GetNotificationListRequest, BasePaginationResponse<IEnumerable<ListNotificationDto>>>
{
    private readonly INotificationService _notificationService;

    public GetNotificationListHandler(INotificationService notificationService)
    {
        _notificationService=notificationService;
    }

    public async Task<BasePaginationResponse<IEnumerable<ListNotificationDto>>> Handle(GetNotificationListRequest request, CancellationToken cancellationToken)
    {
        if (request.IsManager == false)
            return new BasePaginationResponse<IEnumerable<ListNotificationDto>>
            {
                Success = false,
                Message = "You are not manager"
            };

        var notifications = await _notificationService.GetAllWithUser(request.Page, request.Size);
        return notifications;
    }
}