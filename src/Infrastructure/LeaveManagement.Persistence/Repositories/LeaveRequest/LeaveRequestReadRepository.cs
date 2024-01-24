using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.LeaveRequest;

public class LeaveRequestReadRepository : ReadRepository<LeaveManagement.Domain.Entities.LeaveRequest>, ILeaveRequestReadRepository
{
    public LeaveRequestReadRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}