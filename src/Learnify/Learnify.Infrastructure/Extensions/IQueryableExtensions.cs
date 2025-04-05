using System.Linq.Expressions;
using Learnify.Core.Dto.Params;
using Learnify.Core.Specification.Filters;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Core.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> IncludeFields<T>(this IQueryable<T> query, IEnumerable<string> includeStrings)
        where T : class
    {
        if (includeStrings == null)
            return query;

        query = includeStrings?.Aggregate(query, (c, include) => c.Include(include));

        return query;
    }

    public static IQueryable<T> IncludeFields<T>(this IQueryable<T> query,
        IEnumerable<Expression<Func<T, object>>> includeStrings) where T : class
    {
        if (includeStrings == null)
            return query;

        query = includeStrings?.Aggregate(query, (c, include) => c.Include(include));

        return query;
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, EfFilter<T> filter) where T : class
    {
        query = query.IncludeFields(filter.Includes);

        if (filter.Specification is not null)
            query = query.Where(filter.Specification.GetExpression());

        if(filter.OrderByParams is not null && filter.OrderByParams.OrderBy is not null)
            query = query.OrderBy(filter.OrderByParams);

        return query;
    }
    
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, OrderByParams orderByParams)
    {
        if (orderByParams is null)
            return query;

        if (!orderByParams.Asc.HasValue || orderByParams.Asc.Value)
            return query.OrderBy(orderByParams.OrderBy);

        return query.OrderByDescending(orderByParams.OrderBy);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(propertyName, false);
    }

    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(propertyName, true);
    }

    private static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool descending)
    {
        // Get the type of T (the entity type)
        var entityType = typeof(T);

        // Get the property info for the property name passed
        var property = entityType.GetProperty(propertyName);
        if (property == null)
        {
            throw new ArgumentException($"Property {propertyName} does not exist on type {entityType.Name}");
        }

        // Create the parameter expression for the lambda: x =>
        var parameter = Expression.Parameter(entityType, "x");

        // Create the member access expression for the property: x.PropertyName
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);

        // Create the lambda expression: x => x.PropertyName
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        // Choose the method to call (OrderBy or OrderByDescending)
        string methodName = descending ? "OrderByDescending" : "OrderBy";

        // Use the OrderBy or OrderByDescending method dynamically
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { entityType, property.PropertyType },
            source.Expression,
            Expression.Quote(orderByExpression));

        // Apply the expression and return the ordered IQueryable
        return source.Provider.CreateQuery<T>(resultExpression);
    }
}