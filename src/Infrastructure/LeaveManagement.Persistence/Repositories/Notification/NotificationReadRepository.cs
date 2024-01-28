using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;

namespace LeaveManagement.Persistence.Repositories.Notification;

public class NotificationReadRepository : ReadRepository<LeaveManagement.Domain.Entities.Notification>, INotificationReadRepository
{
    public NotificationReadRepository(LeaveManagementDbContext dbContext) : base(dbContext)
    {
    }
}