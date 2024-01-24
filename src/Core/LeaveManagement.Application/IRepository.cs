using LeaveManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application;

public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> Table { get; }
}