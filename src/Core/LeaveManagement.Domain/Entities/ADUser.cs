using LeaveManagement.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Domain.Entities;

public class ADUser : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }

    public Guid? ManagerId { get; set; }
}

public enum UserType
{
    Manager = 30,
    WhiteCollarEmployee = 10,
    BlueCollarEmployee = 20
}