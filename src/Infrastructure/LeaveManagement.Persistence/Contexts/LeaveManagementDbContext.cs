using LeaveManagement.Domain.Entities;
using LeaveManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Contexts;

public class LeaveManagementDbContext : DbContext
{
    public LeaveManagementDbContext(DbContextOptions<LeaveManagementDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaveManagementDbContext).Assembly);
        //base.OnModelCreating(modelBuilder);
    }
    public DbSet<ADUser> ADUsers { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<CumulativeLeaveRequest> CumulativeLeaveRequests { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var changedData = ChangeTracker
             .Entries<BaseEntity>();

        foreach (var data in changedData)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedAt = DateTime.UtcNow,
                EntityState.Modified => data.Entity.LastModifiedAt = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}