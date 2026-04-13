using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace E_Commerce.Infrastructure.Data.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor, ISingletonService
{
    // Catches synchronous SaveChanges() calls
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    // Catches asynchronous SaveChangesAsync() calls (This is what you will use 99% of the time)
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplySoftDelete(DbContext? context)
    {
        if (context is null) return;

        // Look at this beauty! It finds ANY entity that implements your new interface
        var entries = context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            // 1. Stop the hard delete! Tell EF Core we are just modifying the row instead.
            entry.State = EntityState.Modified;

            // 2. Trigger the Delete() method from your interface to update the properties
            entry.Entity.Delete();
        }
    }
}
