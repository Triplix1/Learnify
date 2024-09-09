using System.Linq.Expressions;

namespace Learnify.Core.Extensions;

public static class IQueryableExtensions
{
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