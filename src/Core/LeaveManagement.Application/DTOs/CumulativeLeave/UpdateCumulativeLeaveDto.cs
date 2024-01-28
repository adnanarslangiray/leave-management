﻿using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.CumulativeLeave;

public class UpdateCumulativeLeaveDto
{
    public Guid UserId { get; set; }
    public LeaveTypeEnum LeaveType { get; set; }
    public int TotalHours { get; set; }
    public int Year { get; set; }
}