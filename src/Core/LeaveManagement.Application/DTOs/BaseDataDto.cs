namespace LeaveManagement.Application.DTOs;

public class BaseDataDto<T> where T : class
{
    public T Data { get; set; }
    public int TotalCount { get; set; }
}