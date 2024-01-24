using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.Employee;

public class EmployeeReadRepository : ReadRepository<ADUser>, IEmployeeReadRepository
{
    public EmployeeReadRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}