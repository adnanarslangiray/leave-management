namespace LeaveManagement.Application.DTOs;

public class PagedList<T> where T : class
{
    public T Data { get; set; }
    public int TotalCount { get; set; }

    public PagedList(T data = null, int totalCount = 0)
    {
            
        Data = data;
        TotalCount = totalCount;
    }
}