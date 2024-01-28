using AutoMapper;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs.Employee;
using LeaveManagement.Application.Features.Employee.Requests.Queries;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.Employee.Handlers.Queries;

public class GetEmployeeListHandler : IRequestHandler<GetEmployeeListRequest, BasePaginationResponse<IEnumerable<ListEmployeeDto>>>
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public GetEmployeeListHandler(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService=employeeService;
        _mapper=mapper;
    }

    public async Task<BasePaginationResponse<IEnumerable<ListEmployeeDto>>> Handle(GetEmployeeListRequest request, CancellationToken cancellationToken)
    {
        if (request.IsManager == false)
        {
            return new BasePaginationResponse<IEnumerable<ListEmployeeDto>>
            {
                Success = false,
                Message = "You are not authorized to perform this action"
            };
        }

        var employees = await _employeeService.GetAllEmployee(request.Page, request.Size);

        var employeeList = _mapper.Map<IEnumerable<ListEmployeeDto>>(employees.Data);
        return new BasePaginationResponse<IEnumerable<ListEmployeeDto>>
        {
            Success = true,
            Message = "Employees fetched successfully",
            Data = employeeList,
            CurrentPageIndex = request.Page,
            PageSize = request.Size,
            TotalCount = employees.TotalCount
        };
    }
}