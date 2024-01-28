namespace LeaveManagement.SharedKernel.Constants;

public static class StaticVars
{
    public const int TotalAnnualLeaveHours = 8 * 14 ;
    public const int TotalExcusedLeaveHours = 8 * 5 ;
    public const string ExcusedExceptionMessage = "Excused leave hours cannot be greater than 40 hours";
    public const string AnnualExceptionMessage = "Annual leave hours cannot be greater than 112 hours";
}