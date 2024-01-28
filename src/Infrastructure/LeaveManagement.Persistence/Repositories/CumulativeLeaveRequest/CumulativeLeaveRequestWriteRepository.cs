using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.CumulativeLeaveRequest;

public class CumulativeLeaveRequestWriteRepository : WriteRepository<LeaveManagement.Domain.Entities.CumulativeLeaveRequest>, ICumulativeLeaveRequestWriteRepository
{
    public CumulativeLeaveRequestWriteRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}