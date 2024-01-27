using LeaveManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.LeaveWebAPI.Extensions;

public static class MigrationExtension
{
    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
    {
        try
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<LeaveManagementDbContext>();
            context.Database.Migrate();
            LeaveDbSeed.SeedAsync(context).Wait();

        }
        catch 
        {

        }

        return app;
    }
}