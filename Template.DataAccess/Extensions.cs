using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Template.Contracts.Attribute;
using Template.Contracts.V1.Models;
using Template.DataAccess.Entities;

namespace Template.DataAccess;

public static class Extensions
{
    public static T WithTrackableInfoCreate<T, TR>(this T trackableObject, TR trackableObjectToCopyFromTrackable)
        where T : BaseEntity
        where TR : BaseEntity
    {
        return trackableObject.MapTrackableCreate(trackableObjectToCopyFromTrackable.CreatedBy!,
            trackableObjectToCopyFromTrackable.CreatedOn);
    }

    private static T MapTrackableCreate<T>(this T trackableObject, string userName, DateTimeOffset createdDate)
        where T : BaseEntity
    {
        trackableObject.CreatedBy = userName;
        trackableObject.CreatedOn = createdDate;

        return trackableObject;
    }

    public static Stopwatch StartNow(
        this Stopwatch stopwatch)
    {
        stopwatch.Start();
        return stopwatch;
    }

    public static string GetDescription(this Enum en)
    {
        var type = en.GetType();

        var memInfo = type.GetMember(en.ToString());

        if (memInfo.Length <= 0) return en.ToString();

        var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attrs.Length > 0
            ? ((DescriptionAttribute)attrs[0]).Description
            : en.ToString();
    }

    public static object? GetEnumValueOfAttribute<T>(string? description)
    {
        // Check description 
        if (string.IsNullOrEmpty(description)) return null;

        // Check if the type is Enum 
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException($"Type of '{type}' is not recognized as Enum type");

        // Get all fields and compare the descriptions 
        foreach (var field in type.GetFields())
        {
            if (!(Attribute.GetCustomAttribute
                (field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)) continue;

            if (attribute.Description.ToLower().Equals(description.ToLower()))
                return (T?)field.GetValue(null);
        }

        // Could not find the description in provided Enum Type 
        return null;
    }

    public static IQueryable<TEntity> ApplyFilters<TEntity>(
        this IQueryable<TEntity> entities,
        IEnumerable<Expression<Func<TEntity, bool>>>? filters = null)
    {
        return filters == null
            ? entities
            : filters.Aggregate(entities, (current, filter) => filter != null ? current.Where(filter) : current);
    }

    public static IQueryable<TEntity> ApplyFilter<TEntity>(
        this IQueryable<TEntity> entities,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        if (filter != null) entities = entities.Where(filter);
        return entities;
    }

    public static IQueryable<TEntity> ApplyIncludes<TEntity>(
        this IQueryable<TEntity> entities,
        IEnumerable<Expression<Func<TEntity, object>>>? includes)
        where TEntity : class
    {
        if (includes != null)
            foreach (var include in includes)
                entities = entities.Include(include);
        return entities;
    }

    public static IQueryable<TEntity> ApplyIncludes<TEntity>(
        this IQueryable<TEntity> entities,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes)
        where TEntity : class
    {
        if (includes != null) entities = includes.Invoke(entities);
        return entities;
    }

    public static IEnumerable<TEntity> ApplyPagingAndGet<TEntity>(
        this IQueryable<TEntity> entities,
        IListFilter? pagingFilter) where TEntity : class
    {
        if (pagingFilter != null)
            entities = entities.Skip(pagingFilter.Skip ?? 0)
                .Take(pagingFilter.Take ?? 10000);
        return entities.AsEnumerable();
    }

    public static IQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> queryable,
        Type resourceType,
        string? orderBy, Expression<Func<TSource, TKey>> defaultOrder)
    {
        return string.IsNullOrWhiteSpace(orderBy)
            ? queryable.OrderBy(defaultOrder)
            : queryable.OrderBy(resourceType, orderBy);
    }

    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> queryable,
        Type resourceType,
        string? orderBy)
    {
        if (orderBy == null) return queryable;

        //split the orderby string by comma
        //key is the column name without the + or -
        // value is + or - (+ for ascending and - for descending)
        var dictionary = orderBy.Split(',')
            .ToDictionary(item => item.Replace(new[]
            {
                "+",
                "-"
            }, ""), item => item.Left(1));

        //IOrderedQuerable<T> to order by more than one column using ThenBy and ThenByDescending
        IOrderedQueryable<T>? orderedResult = null;
        var param = Expression.Parameter(typeof(T), "e");
        //for each column name in the resouce, get the column name in the entity (what we specify in the IncludeInOrderByAttribute)
        foreach (var pair in dictionary)
        {
            var exp = CreateOrderbyExpression(resourceType, param, pair);

            //order by descending if - otherwise ascending
            if (orderedResult == null)
                orderedResult = pair.Value == "-"
                    ? queryable.OrderByDescending(
                        Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param))
                    : queryable.OrderBy(
                        Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param));
            else
                orderedResult = pair.Value == "-"
                    ? orderedResult.ThenByDescending(
                        Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param))
                    : orderedResult.ThenBy(
                        Expression.Lambda<Func<T, object>>(Expression.Convert(exp, typeof(object)), param));
        }

        return orderedResult ?? queryable;
    }

    public static IQueryable<TEntity> ApplySorting<TEntity, TResource>(
        this IQueryable<TEntity> entities,
        IListFilter? pagingFilter)
    {
        return string.IsNullOrEmpty(pagingFilter?.OrderBy)
            ? entities
            : entities.OrderBy(typeof(TResource), pagingFilter.OrderBy);
    }

    public static string Left(this string value, int length)
    {
        return value[..Math.Min(length, value.Length)];
    }

    private static Expression CreateOrderbyExpression(Type resourceType, ParameterExpression param,
        KeyValuePair<string, string> pair)
    {
        Expression exp;

        if (pair.Key.Contains('.'))
        {
            exp = param;
            foreach (var item in pair.Key.Split('.'))
                exp = Expression.PropertyOrField(exp, item);
        }
        else
        {
            var entityPropertyName = resourceType.GetPropertiesWithAttribute<IncludeInOrderByAttribute>()
                .First(p => string.Equals(p.Property.Name, pair.Key, StringComparison.CurrentCultureIgnoreCase))
                .Attribute.EntityPropertyName;
            exp = Expression.PropertyOrField(param, entityPropertyName!);
        }

        return exp;
    }

    public static IEnumerable<PropertyAttribute<TAttribute>> GetPropertiesWithAttribute<TAttribute>(this Type member)
        where TAttribute : Attribute
    {
        return member
            .GetTypeInfo()
            .GetProperties()
            .Select(p => new PropertyAttribute<TAttribute>
            {
                Attribute = p.GetAttribute<TAttribute>(),
                Property = p
            });
    }

    public static string Replace(this string target, ICollection<string> oldValues, string newValue)
    {
        oldValues.Each(oldValue => target = target.Replace(oldValue, newValue));
        return target;
    }

    public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
    {
        foreach (var obj in instance)
            action(obj);
    }

    public class PropertyAttribute<TAttribute>
    {
        public required TAttribute Attribute { get; set; }
        public required PropertyInfo Property { get; set; }
    }
}