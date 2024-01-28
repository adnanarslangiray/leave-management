using LeaveManagement.Application.Features.Employee.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.LeaveWebAPI.Controllers;

[Route("api")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediatr;

    public UsersController(IMediator mediatr)
    {
        _mediatr=mediatr;
    }

    [HttpGet]
    [Route("users")]
    public async Task<IActionResult> GetUsers([FromQuery] GetEmployeeListRequest request)
    {
        var response = await _mediatr.Send(request);
        return Ok(response);
    }
}