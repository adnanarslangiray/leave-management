using LeaveManagement.Application.DTOs.CumulativeLeave;
using LeaveManagement.Application.Events.DomainEvents.Interfaces;

namespace LeaveManagement.Application.Events.DomainEvents.Concretes;

public class CreateCumulativeLeaveRequestEvent : IDomainEvent
{
    public CreateCumulativeLeaveDto CreateCumulativeLeaveDto { get; set; }
}