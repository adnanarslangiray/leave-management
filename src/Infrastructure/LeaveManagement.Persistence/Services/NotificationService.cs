using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Persistence.Contexts;
using LeaveManagement.SharedKernel.Utilities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationReadRepository _notificationReadRepository;
    private readonly LeaveManagementDbContext _dbContext;

    public NotificationService(INotificationReadRepository notificationReadRepository, LeaveManagementDbContext dbContext)
    {
        _notificationReadRepository=notificationReadRepository;
        _dbContext=dbContext;
    }

    public async Task<PagedList<IEnumerable<Notification>>> GetAll(int page, int size)
    {
        var query = _notificationReadRepository.Table.AsNoTracking();

        //paged
        var list = query.Skip((page - 1) * size).Take(size);

        return new PagedList<IEnumerable<Notification>>(list, await query.CountAsync());
    }

    public async Task<BasePaginationResponse<IEnumerable<ListNotificationDto>>> GetAllWithUser(int page, int size)
    {
        var query = from ntf in _dbContext.Notifications
                    join adu in _dbContext.ADUsers on ntf.UserId equals adu.Id
                    orderby ntf.Year descending
                    select new ListNotificationDto
                    {
                        FullName = adu.FullName,
                        Id = ntf.Id,
                        Year = ntf.Year,
                        CreatedDate = ntf.CreatedAt,
                        Message = ntf.Message,
                    };

        var list = query.Skip((page - 1) * size).Take(size).ToList();
        var count = await query.CountAsync();
        return new BasePaginationResponse<IEnumerable<ListNotificationDto>>(list, list != null, "", page, count, size);
    }
}