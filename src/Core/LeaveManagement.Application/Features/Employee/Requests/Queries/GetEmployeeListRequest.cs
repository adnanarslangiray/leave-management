using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.DTOs.Employee;
using LeaveManagement.SharedKernel.Utilities;
using MediatR;

namespace LeaveManagement.Application.Features.Employee.Requests.Queries;

public class GetEmployeeListRequest : IRequest<BasePaginationResponse<IEnumerable<ListEmployeeDto>>>
{
    public bool IsManager { get; set; } = true;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}