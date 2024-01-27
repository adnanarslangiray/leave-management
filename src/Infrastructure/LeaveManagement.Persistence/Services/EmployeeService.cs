using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Repositories;
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
}