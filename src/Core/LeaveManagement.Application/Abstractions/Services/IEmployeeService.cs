using LeaveManagement.Application.DTOs;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Abstractions.Services;

public interface IEmployeeService
{
    Task<string> GetEmployeeNamebyId(string userId);

    Task<PagedList<IEnumerable<ADUser>>> GetAllEmployee(int page, int size);
}