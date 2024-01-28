using LeaveManagement.SharedKernel.Enums;

namespace LeaveManagement.Application.DTOs.CumulativeLeave;

public class ListCumulativeLeaveRequestDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string LeaveType { get; set; }
    public int TotalHours { get; set; }
    public int Year { get; set; }
}