using LeaveManagement.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.DTOs.LeaveRequest;

public class LeaveRequestDto : BaseDto
{
    public int FormNumber { get; set; }
    public int RequestNumber { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public string Reason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public WorkflowStatusEnum WorkflowStatus { get; set; }
    public Guid AssignedUserId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid LastModifiedById { get; set; }
}
