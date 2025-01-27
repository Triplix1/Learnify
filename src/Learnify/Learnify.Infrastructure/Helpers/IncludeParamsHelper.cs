using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Helpers;

public static class IncludeParamsHelper
{
    public static IQueryable<T> IncludeStrings<T>(IEnumerable<string> includeStrings, IQueryable<T> query) where T : class
    {
        query = includeStrings?.Aggregate(query, (c, include) => c.Include(include));

        return query;
    }
    
    public static IQueryable<T> IncludeFields<T>(IEnumerable<Expression<Func<T,object>>> includeStrings, IQueryable<T> query) where T : class
    {
        query = includeStrings?.Aggregate(query, (c, include) => c.Include(include));

        return query;
    }
}