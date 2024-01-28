using LeaveManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Persistence.Extensions;

public static class DetachLocalExtension
{
    public static void DetachLocal<T>(this DbContext context, T t, Guid entryId)
   where T : BaseEntity
    {
        var local = context.Set<T>()
            .Local
            .FirstOrDefault(entry => entry.Id.Equals(entryId));
        if (local is not null)
        {
            context.Entry(local).State = EntityState.Detached;
        }
        context.Entry(t).State = EntityState.Modified;
    }
}