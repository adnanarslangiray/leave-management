using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries;

public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestListRequest, BasePaginationResponse<List<LeaveRequestDto>>>
{
    private readonly ILeaveRequestService _leaveRequestService;
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public GetLeaveRequestListHandler(ILeaveRequestService leaveRequestService, IMapper mapper, IEmployeeService employeeService)
    {
        _leaveRequestService=leaveRequestService;
        _mapper=mapper;
        _employeeService=employeeService;
    }

    public async Task<BasePaginationResponse<List<LeaveRequestDto>>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
    {
        if (request.IsManager == false)
            return new BasePaginationResponse<List<LeaveRequestDto>>
            {
                Success = false,
                Message = "You are not manager"
            };

        var result = await _leaveRequestService.GetLeaveRequestByUserId(request.EmployeeId, request.Page, request.Size);
        var leaveRequestDtos = _mapper.Map<List<LeaveRequestDto>>(result.Data);
        var employeeName = await _employeeService.GetEmployeeNamebyId(request.EmployeeId.ToString());
        leaveRequestDtos?.ForEach(x =>{x.FullName = employeeName;});
        var response = new BasePaginationResponse<List<LeaveRequestDto>>(leaveRequestDtos, true, "", request.Page, result.TotalCount, request.Size);
        return response;
    }
}