using LeaveManagement.Application;
using LeaveManagement.Persistence.Contexts;
using LeaveManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LeaveManagement.Persistence.Repositories;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    private readonly LeaveManagementDbContext _dbContext;

    public WriteRepository(LeaveManagementDbContext dbContext)
    {
        _dbContext=dbContext;
    }

    public DbSet<T> Table => _dbContext.Set<T>();

    public async Task<bool> AddAsync(T model)
    {
        EntityEntry<T> entityEntry = await Table.AddAsync(model);
        var result = entityEntry.State == EntityState.Added;
        return result;
    }

    public async Task<bool> AddRangeAsync(List<T> data)
    {
        await Table.AddRangeAsync(data);
        return true;
    }

    public bool Remove(T model)
    {
        EntityEntry<T> entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        T model = await Table.FindAsync(Guid.Parse(id));
        if (model is null)
            return false;
        return Remove(model);
    }

    public bool RemoveRange(List<T> data)
    {
        Table.RemoveRange(data);
        return true;
    }

    public bool Update(T model)
    {
        //_dbContext.DetachLocal(model, model.Id);
        EntityEntry<T> entityEntry = Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }

    public async Task<int> SaveAsync()
        => await _dbContext.SaveChangesAsync();
}