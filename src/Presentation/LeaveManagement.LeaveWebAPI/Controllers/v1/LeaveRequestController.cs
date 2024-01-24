using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.LeaveWebAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
    }
}