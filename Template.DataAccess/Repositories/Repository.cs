using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Template.Contracts.V1.Models;

namespace Template.DataAccess.Repositories;

/// <summary>
///     Repository that works with an entity that implements ICorrelateBy.
/// </summary>
/// <typeparam name="TEntity">Entity.</typeparam>
/// <typeparam name="TEntityId">Entity id.</typeparam>
public class Repository<TEntity, TEntityId> : Repository<TEntity>,
    IRepository<TEntity, TEntityId>
    where TEntity : class, ICorrelateBy<TEntityId>
{
    public Repository(
        DbContext context)
        : base(context)
    {
    }

    public virtual Task<TEntity?> GetByIdAsync(
        TEntityId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
    {
        return GetQueryable()
            .ApplyIncludes(includes)
            .FirstOrDefaultAsync(e => e!.Id!.Equals(id));
    }

    public virtual Task<TEntity?> GetByIdTrackingAsync(
        TEntityId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
    {
        return GetTrackingQueryable()
            .ApplyIncludes(includes)
            .FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    public void UpdateNoTracking(TEntity entity)
    {
        Update(entity);
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Context.Database.CreateExecutionStrategy();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    public virtual void CommitTransaction()
    {
        Context.Database.CommitTransaction();
    }

    public virtual void RollBackTransaction()
    {
        Context.Database.RollbackTransaction();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return Context.Database.BeginTransactionAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        return Context.Database.BeginTransactionAsync(isolationLevel);
    }

    public bool HasTransaction()
    {
        return Context.Database.CurrentTransaction != null;
    }

    public void MarkPropertyUnchanged<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertySelector)
    {
        Context.Entry(entity).Property(propertySelector).IsModified = false;
    }

    public void MarkUnchanged<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class
    {
        Context.Entry(entity).State = EntityState.Unchanged;
    }

    public void MarkModified<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class
    {
        Context.Entry(entity).State = EntityState.Modified;
    }

    public void MarkDeleted<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class
    {
        Context.Entry(entity).State = EntityState.Deleted;
    }

    public virtual Task<TEntity?> LastOrDefaultAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        Expression<Func<TEntity, object>>? orderByDescendingColumnSelector = null)
    {
        if (orderByDescendingColumnSelector == null) orderByDescendingColumnSelector = e => e.Id!;

        return GetQueryable()
            .ApplyFilters(filters)
            .ApplyIncludes(funcIncludes)
            .OrderByDescending(orderByDescendingColumnSelector)
            .FirstOrDefaultAsync();
    }

    public Task AddRangeAsync(IList<TEntity> entities)
    {
        return Context.Set<TEntity>().AddRangeAsync(entities);
    }

    public void DetachAllRecords()
    {
        var entities = Context.ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Detached)
            .ToList();

        foreach (var entry in entities)
            entry.State = EntityState.Detached;
    }

    public void DetachAllOfType()
    {
        var entities = Context.ChangeTracker.Entries()
            .Where(e =>
                e.State != EntityState.Detached &&
                e.Entity is TEntity)
            .ToList();

        foreach (var entry in entities)
            entry.State = EntityState.Detached;
    }
}

/// <summary>
///     Repository that works with an entity.
/// </summary>
/// <typeparam name="TEntity">Entity.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    public Repository(
        DbContext context)
    {
        Context = context;
    }

    protected virtual DbContext Context { get; }

    public virtual Task<List<TEntity>> GetItemsAsync<TResource>(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null)
    {
        var result = GetQueryable()
            .ApplyFilters(filters)
            .ApplyIncludes(funcIncludes)
            .ApplySorting<TEntity, TResource>(pagingFilter)
            .ApplyPagingAndGet(pagingFilter);

        return Task.FromResult(result.ToList());
    }

    public virtual Task<List<TEntity>> GetItemsAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null)
    {
        var query = GetQueryable();
        if (orderByType != null &&
            !string.IsNullOrWhiteSpace(orderBy))
            query = query.OrderBy(orderByType, orderBy);

        var result = query
            .ApplyFilters(filters)
            .ApplyIncludes(funcIncludes)
            .ApplyPagingAndGet(pagingFilter);

        return Task.FromResult(result.ToList());
    }

    public virtual Task<TEntity[]> GetItemsAsync(
        IEnumerable<Expression<Func<TEntity, bool>>>? listFilters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null,
        bool isTrackable = false)
    {
        var query = isTrackable ? GetTrackingQueryable() : GetQueryable();
        if (orderByType != null &&
            !string.IsNullOrWhiteSpace(orderBy))
            query = query.OrderBy(orderByType, orderBy);
        var result = query
            .ApplyFilters(listFilters)
            .ApplyIncludes(funcIncludes)
            .ApplyPagingAndGet(pagingFilter);

        return Task.FromResult(result.ToArray());
    }

    public virtual Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
    {
        return GetQueryable()
            .ApplyFilters(filters)
            .ApplyIncludes(includes)
            .FirstOrDefaultAsync();
    }

    public virtual Task<int> CountAsync(
        params Expression<Func<TEntity, bool>>[] filters)
    {
        return GetQueryable()
            .ApplyFilters(filters)
            .CountAsync();
    }

    public virtual Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>>[]? filters,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null)
    {
        return GetQueryable()
            .ApplyIncludes(includes)
            .ApplyFilters(filters)
            .AnyAsync();
    }

    public Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }

    public void Update(
        TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void Add(
        TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void Delete(
        TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(
        Expression<Func<TEntity, bool>> predicate)
    {
        Context.Set<TEntity>()
            .RemoveRange
            (Context.Set<TEntity>()
                .Where(predicate));
    }

    public virtual IQueryable<TEntity> GetDbContext()
    {
        return Context.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return Context.Set<TEntity>().AsNoTracking();
    }

    protected virtual IQueryable<TEntity> GetTrackingQueryable()
    {
        return Context.Set<TEntity>();
    }

    public virtual Task<List<TEntity>> GetItemsOrderDescendingAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Expression<Func<TEntity, object>>? orderByDescendingColumnSelector = null)
    {
        var result = GetQueryable()
            .ApplyFilters(filters)
            .ApplyIncludes(funcIncludes)
            .OrderByDescending(orderByDescendingColumnSelector!)
            .ApplyPagingAndGet(pagingFilter);

        return Task.FromResult(result.ToList());
    }

    public virtual Task<TEntity?> FirstOrDefaultAsync(
        IEnumerable<Expression<Func<TEntity, bool>>>? listFilters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        Type? orderByType = null,
        string? orderBy = null,
        bool isTrackable = false)
    {
        var query = isTrackable ? GetTrackingQueryable() : GetQueryable();
        if (orderByType != null &&
            !string.IsNullOrWhiteSpace(orderBy))
            query = query.OrderBy(orderByType, orderBy);

        return query
            .ApplyFilters(listFilters)
            .ApplyIncludes(funcIncludes)
            .FirstOrDefaultAsync();
    }

    public virtual Task<TEntity?> FirstOrDefaultTrackingAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        IEnumerable<Expression<Func<TEntity, object>>>? listIncludes = null)
    {
        return GetTrackingQueryable()
            .ApplyFilter(filter)
            .ApplyIncludes(listIncludes)
            .FirstOrDefaultAsync();
    }

    public virtual Task<TEntity?> FirstOrDefaultTrackingAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null)
    {
        return GetTrackingQueryable()
            .ApplyFilter(filter)
            .ApplyIncludes(funcIncludes)
            .FirstOrDefaultAsync();
    }

    public virtual bool Exists(
        Func<TEntity, bool> filters)
    {
        return GetQueryable()
            .Any(filters);
    }

    public void Attach(TEntity entity)
    {
        Context.Attach(entity);
    }
}