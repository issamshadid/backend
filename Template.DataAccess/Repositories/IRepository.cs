using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Template.Contracts.V1.Models;

namespace Template.DataAccess.Repositories;

/// <summary>
///     Repository contract to work with an entity that implements ICorrelateBy.
/// </summary>
/// <typeparam name="TEntity">Entity.</typeparam>
/// <typeparam name="TEntityId">Entity id.</typeparam>
public interface IRepository<TEntity, in TEntityId>
    where TEntity : class, ICorrelateBy<TEntityId>
{
    Task<List<TEntity>> GetItemsAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null);

    Task<List<TEntity>> GetItemsAsync<TResource>(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null);

    Task<TEntity[]> GetItemsAsync(
        IEnumerable<Expression<Func<TEntity, bool>>>? listFilters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null,
        bool isTrackable = false);

    Task<List<TEntity>> GetItemsOrderDescendingAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Expression<Func<TEntity, object>>? orderByDescendingColumnSelector = null);

    Task<TEntity?> GetByIdAsync(
        TEntityId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    Task<TEntity?> GetByIdTrackingAsync(
        TEntityId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    Task<TEntity?> FirstOrDefaultAsync(
        IEnumerable<Expression<Func<TEntity, bool>>>? listFilters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        Type? orderByType = null,
        string? orderBy = null,
        bool isTrackable = false);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    Task<TEntity?> FirstOrDefaultTrackingAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        IEnumerable<Expression<Func<TEntity, object>>>? listIncludes = null);

    Task<TEntity?> FirstOrDefaultTrackingAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null);

    Task<TEntity?> LastOrDefaultAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        Expression<Func<TEntity, object>>? orderByDescendingColumnSelector = null);

    Task<int> CountAsync(
        params Expression<Func<TEntity, bool>>[] filters);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    bool Exists(
        Func<TEntity, bool> filters);

    Task SaveAsync();

    void Delete(
        TEntity entity);

    void Update(
        TEntity entity);

    void UpdateNoTracking(TEntity entity);

    void Add(
        TEntity entity);

    Task AddRangeAsync(IList<TEntity> entities);

    void Attach(TEntity entity);

    IExecutionStrategy CreateExecutionStrategy();

    IDbContextTransaction BeginTransaction();

    bool HasTransaction();

    void CommitTransaction();

    void RollBackTransaction();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);

    void MarkPropertyUnchanged<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertySelector);

    void MarkModified<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class;

    void MarkUnchanged<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class;

    void MarkDeleted<TSubEntity>(
        TSubEntity entity)
        where TSubEntity : class;

    void RemoveRange(
        Expression<Func<TEntity, bool>> predicate);

    void DetachAllRecords();

    void DetachAllOfType();

    IQueryable<TEntity> GetDbContext();
}

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetItemsAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null);

    Task<List<TEntity>> GetItemsAsync<TResource>(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null);

    Task<TEntity[]> GetItemsAsync(
        IEnumerable<Expression<Func<TEntity, bool>>>? listFilters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? funcIncludes = null,
        IListFilter? pagingFilter = null,
        Type? orderByType = null,
        string? orderBy = null,
        bool isTrackable = false);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    Task<int> CountAsync(
        params Expression<Func<TEntity, bool>>[] filters);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>>[] filters,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null);

    Task SaveAsync();

    void Delete(
        TEntity entity);

    void Update(
        TEntity entity);

    void Add(
        TEntity entity);

    void RemoveRange(
        Expression<Func<TEntity, bool>> predicate);
}