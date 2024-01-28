using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.Notifications.Requests.Queries;

public class GetNotificationListRequest : IRequest<BasePaginationResponse<IEnumerable<ListNotificationDto>>>
{
    public bool IsManager { get; set; } = true;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}