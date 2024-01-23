using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Persistence.Contexts;

public class LeaveDbSeed
{
    public static async Task SeedAsync(LeaveManagementDbContext leaveManagementDbContext)
    {
        if (leaveManagementDbContext.ADUsers.Any() == false)
        {
            leaveManagementDbContext.ADUsers.AddRange(GetPreconfiguredADUsers());
            await leaveManagementDbContext.SaveChangesAsync();
        }
    }

    private static IEnumerable<ADUser> GetPreconfiguredADUsers()
    {
        var managerId = Guid.NewGuid();
        var whiteCollarId = Guid.NewGuid();
        var blueCollarId = Guid.NewGuid();

        var adUsers = new List<ADUser>
            {
                new ADUser
                {
                    Id = managerId,
                    FirstName = "Münir",
                    LastName = "Özkul",
                    Email = "munir.ozkul@negzel.com",
                    UserType = UserType.Manager,
                },
                 new ADUser
                 {
                    Id = whiteCollarId,
                    FirstName = "Şener",
                    LastName = "Şen",
                    Email = "sener.sen@negzel.com",
                    ManagerId = managerId,
                    UserType = UserType.WhiteCollarEmployee,
                 },
                 new ADUser
                 {
                    Id = blueCollarId,
                    FirstName = "Kemal",
                    LastName = "Sunal",
                    Email = "kemal.sunal@negzel.com",
                    ManagerId = whiteCollarId,
                    UserType = UserType.BlueCollarEmployee,
                 }
            };
        return adUsers;
    }
}