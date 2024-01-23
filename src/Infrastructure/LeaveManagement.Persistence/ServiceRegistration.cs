using LeaveManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LeaveManagementDbContext>(options
               => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"),
               b => b.MigrationsAssembly(typeof(LeaveManagementDbContext).Assembly.FullName)));
    }
}