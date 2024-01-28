using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.Events.DomainEvents.Interfaces;

namespace LeaveManagement.Application.Events.DomainEvents.Concretes;

public class UpdateCumulativeLeaveRequestEvent : IDomainEvent
{
    public UpdateCumulativeLeaveDto UpdateCumulativeLeaveDto { get; set; }
}