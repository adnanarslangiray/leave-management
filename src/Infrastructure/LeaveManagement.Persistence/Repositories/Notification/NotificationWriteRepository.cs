using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.Notification;

public class NotificationWriteRepository : WriteRepository<LeaveManagement.Domain.Entities.Notification>, INotificationWriteRepository
{
    public NotificationWriteRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}
