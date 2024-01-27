namespace LeaveManagement.Application.Abstractions.Services;

public interface IEmployeeService
{
    Task<string> GetEmployeeNamebyId(string userId);
}