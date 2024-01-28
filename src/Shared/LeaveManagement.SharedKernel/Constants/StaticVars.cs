namespace LeaveManagement.SharedKernel.Constants;

public static class StaticVars
{
    public const int TotalAnnualLeaveHours = 8 * 14;
    public const int TotalExcusedLeaveHours = 8 * 5;
    public const string ExcusedExceptionMessage = "Excused leave hours cannot be greater than 40 hours";
    public const string AnnualExceptionMessage = "Annual leave hours cannot be greater than 112 hours";

    public static int CalculateToTalHours(DateTime startDate, DateTime endDate)
    => Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(day => startDate.AddDays(day))
            .Count(date => date.DayOfWeek != DayOfWeek.Sunday) * 8;

}