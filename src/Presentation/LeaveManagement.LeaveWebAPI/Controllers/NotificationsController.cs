using LeaveManagement.Application.Features.Notifications.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.LeaveWebAPI.Controllers;

[Route("api")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediatr;

    public NotificationsController(IMediator mediator)
    {
        _mediatr=mediator;
    }

    [HttpGet]
    [Route("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationListRequest request)
    {
        var notifications = await _mediatr.Send(request);
        return Ok(notifications);
    }


}