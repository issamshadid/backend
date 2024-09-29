using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Template.DataAccess.Entities;

namespace Template.DataAccess;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChanges(eventData, result);

        var entities = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        var nameClaim = _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == "name")?.Value;

        var currentUserName = nameClaim ?? "System";

        foreach (var entry in entities)
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                    entry.Entity.CreatedBy = currentUserName;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                    entry.Entity.ModifiedBy = currentUserName;
                    break;
                case EntityState.Deleted:
                    // Switch to soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedOn = DateTimeOffset.UtcNow;
                    entry.Entity.DeletedBy = currentUserName;

                    // Handle soft delete for related entities
                    SoftDeleteRelatedEntities(context, entry.Entity, currentUserName);
                    break;
            }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        return new ValueTask<InterceptionResult<int>>(Task.FromResult(SavingChanges(eventData, result)));
    }

    private static void SoftDeleteRelatedEntities(DbContext context, BaseEntity entity, string deletedBy)
    {
        if (entity is CategoryEntity category)
        {
            // Add your implementation here
        }
    }
}