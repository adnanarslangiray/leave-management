using LeaveManagement.Application.Features.CumulativeLeaveRequest.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.LeaveWebAPI.Controllers;

[Route("api")]
[ApiController]
public class CumulativeRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CumulativeRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    [Route("cumulative-requests")]
    public async Task<IActionResult> GetCumulativeRequests([FromQuery] GetCumulativeLeaveRequestListRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    
}