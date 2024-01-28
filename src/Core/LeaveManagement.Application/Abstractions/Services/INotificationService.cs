using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel.Utilities;

namespace LeaveManagement.Application.Abstractions.Services;

public interface INotificationService
{
    Task<PagedList<IEnumerable<Notification>>> GetAll(int page, int size);
    Task<BasePaginationResponse<IEnumerable<ListNotificationDto>>> GetAllWithUser(int page, int size);
}