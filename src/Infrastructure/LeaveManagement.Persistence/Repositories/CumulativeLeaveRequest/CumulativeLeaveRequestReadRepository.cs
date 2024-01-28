using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.CumulativeLeaveRequest;

public class CumulativeLeaveRequestReadRepository : ReadRepository<LeaveManagement.Domain.Entities.CumulativeLeaveRequest>, ICumulativeLeaveRequestReadRepository
{
    public CumulativeLeaveRequestReadRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}