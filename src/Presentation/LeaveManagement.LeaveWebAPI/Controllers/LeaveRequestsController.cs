using Asp.Versioning;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.LeaveWebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("leave-requests/{id}")]
        public IActionResult GetLeaveRequestById(string id)
        {
            return Ok("LeaveRequestController");
        }

        [HttpPost("leave-requests")]
        public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestCommand request)
        {

            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("leave-requests")]
        public async Task<IActionResult> GetLeaveRequestByUserId([FromQuery] GetLeaveRequestListRequest request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}