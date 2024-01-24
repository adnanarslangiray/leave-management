using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.LeaveRequest;

public class LeaveRequestWriteRepository : WriteRepository<LeaveManagement.Domain.Entities.LeaveRequest>, ILeaveRequestWriteRepository
{
    public LeaveRequestWriteRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}