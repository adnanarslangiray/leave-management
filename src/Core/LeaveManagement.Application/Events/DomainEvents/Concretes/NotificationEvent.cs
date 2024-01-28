using LeaveManagement.Application.DTOs.Notifications;
using LeaveManagement.Application.Events.DomainEvents.Interfaces;

namespace LeaveManagement.Application.Events.DomainEvents.Concretes;

public class NotificationEvent : IDomainEvent
{
    public CreateNotificationDto CreateNotificationDto { get; set; }
    public bool IsSendingManagerNotification { get; set; }
}