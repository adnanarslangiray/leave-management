using LeaveManagement.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.DTOs.LeaveRequest;

public class LeaveRequestDto : BaseDto
{
    public string RequestFormNumber { get; set; }
    public string FullName { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public string Reason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public WorkflowStatusEnum WorkflowStatus { get; set; }
    public int TotalHours { get; set; }
}