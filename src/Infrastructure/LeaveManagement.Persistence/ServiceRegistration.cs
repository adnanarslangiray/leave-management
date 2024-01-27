using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Repositories;
using LeaveManagement.Persistence.Contexts;
using LeaveManagement.Persistence.Repositories.Employee;
using LeaveManagement.Persistence.Repositories.LeaveRequest;
using LeaveManagement.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LeaveManagement.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LeaveManagementDbContext>(options
               => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"),
               b => b.MigrationsAssembly(typeof(LeaveManagementDbContext).Assembly.FullName)),
               ServiceLifetime.Transient, ServiceLifetime.Transient);

        //repositories
        services.AddSingleton<ILeaveRequestReadRepository, LeaveRequestReadRepository>();
        services.AddSingleton<ILeaveRequestWriteRepository, LeaveRequestWriteRepository>();
        services.AddSingleton<IEmployeeReadRepository, EmployeeReadRepository>();

        //services
        services.AddSingleton<ILeaveRequestService, LeaveRequestService>();
        services.AddSingleton<IEmployeeService, EmployeeService>();

    }
}