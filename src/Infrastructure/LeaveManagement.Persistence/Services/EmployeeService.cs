using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeReadRepository _employeeReadRepository;

    public EmployeeService(IEmployeeReadRepository employeeReadRepository)
    {
        _employeeReadRepository=employeeReadRepository;
    }

    public async Task<string> GetEmployeeNamebyId(string userId)
    {
        var employee = await _employeeReadRepository.Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));

        return employee?.FullName;
    }
    //get all employee paged

    public async Task<PagedList<IEnumerable<ADUser>>> GetAllEmployee(int page, int size)
    {
        var query = _employeeReadRepository.Table.AsNoTracking();
        var list = query.Skip((page - 1) * size).Take(size);
        var totalCount = await query.CountAsync();
        return new PagedList<IEnumerable<ADUser>>(list, totalCount);

    }
}