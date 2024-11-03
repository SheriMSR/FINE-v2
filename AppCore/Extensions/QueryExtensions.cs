using System.Linq.Expressions;
using AppCore.Models;

namespace AppCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDescending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.Type },
            source.Expression,
            Expression.Quote(lambda)
        );

        return source.Provider.CreateQuery<T>(resultExpression);
    }

    public static IOrderedQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
    {
        if (string.IsNullOrEmpty(orderBy))
            return (IOrderedQueryable<T>)source;
        var isDescending = orderBy.Split(' ').Last().ToLowerInvariant()
            .StartsWith("desc");

        var sortField = orderBy.Split(' ').First();

        // Sort
        if (!string.IsNullOrEmpty(sortField))
            try
            {
                source = source.OrderBy(sortField, isDescending);
            }
            catch
            {
                throw new ApiException(MessageKey.BadRequest, StatusCode.BAD_REQUEST);
            }

        return (IOrderedQueryable<T>)source;
    }

    public static List<TSource> GetAllChildItems<TSource, TKey>(this IQueryable<TSource> source,
        TSource parentItem,
        Expression<Func<TSource, TKey>> childKeySelector,
        Expression<Func<TSource, TKey>> parentKeySelector)
    {
        var queue = new Queue<TSource>();
        queue.Enqueue(parentItem);

        var childItems = new List<TSource>();
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            childItems.Add(item);

            var childKey = childKeySelector.Compile();
            var parentKey = parentKeySelector.Compile();

            foreach (var childItem in source.ToList()
                         .Where(x => childKey(x) != null && childKey(x).Equals(parentKey(item))))
            {
                queue.Enqueue(childItem);
            }
        }

        childItems.Remove(parentItem);
        return childItems;
    }
}