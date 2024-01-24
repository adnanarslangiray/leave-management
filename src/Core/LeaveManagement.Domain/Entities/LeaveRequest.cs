using LeaveManagement.SharedKernel;
using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Domain.Entities;

public class LeaveRequest : BaseEntity
{
    public string FormNumber { get; set; }
    public string RequestNumber { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public string Reason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public WorkflowStatusEnum WorkflowStatus { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid LastModifiedById { get; set; }
}