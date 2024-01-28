using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.DTOs.Employee;

public class ListEmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public UserType UserType { get; set; }
    public Guid? ManagerId { get; set; }
}