using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Repositories;

public interface ILeaveRequestWriteRepository : IWriteRepository<LeaveRequest>
{
}