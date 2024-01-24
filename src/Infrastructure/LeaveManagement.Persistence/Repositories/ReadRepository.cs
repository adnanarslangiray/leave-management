using LeaveManagement.Application;
using LeaveManagement.Persistence.Contexts;
using LeaveManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LeaveManagement.Persistence.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly LeaveManagementDbContext _dbContext;

    public ReadRepository(LeaveManagementDbContext dbContext)
    {
        _dbContext=dbContext;
    }

    public DbSet<T> Table => _dbContext.Set<T>();

    public IQueryable<T> GetAll(bool tracking = true)
    {
        return tracking
            ? Table.AsQueryable()
            : Table.AsNoTracking();
    }

    public async Task<T> GetByIdAsync(string id, bool tracking = true)
    {
        return tracking
            ? await Table.AsQueryable().FirstOrDefaultAsync(data => data.Id == Guid.Parse(id))
            : await Table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        return tracking
                ? await Table?.AsQueryable()?.FirstOrDefaultAsync(method)
                : await Table?.AsNoTracking()?.FirstOrDefaultAsync(method);
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        return tracking
            ? Table?.AsQueryable()?.Where(method)
            : Table?.AsNoTracking()?.Where(method);
    }
}