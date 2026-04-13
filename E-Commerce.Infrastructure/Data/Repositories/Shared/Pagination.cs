using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.Data.Repositories.Shared;

public static class Pagination
{
    public static async Task<(IReadOnlyCollection<T> Items, TKey? NextCursor)>
    PaginateWithCursorAsync<T, TKey>(
        this IQueryable<T> query,
        Expression<Func<T, TKey>> keySelector,
        TKey? lastCursor,
        int size,
        int maxSize = 50,
        CancellationToken ct = default)
        where TKey : struct, IComparable<TKey>
    {
        var validSize = size < 1 ? 10 : Math.Min(size, maxSize);

        if (lastCursor.HasValue)
        {
            query = query.Where(
                BuildCursorExpression(keySelector, lastCursor.Value)
            );
        }

        var items = await query
            .OrderBy(keySelector)
            .Take(validSize)
            .ToListAsync(ct);

        TKey? nextCursor = items.Any()
            ? keySelector.Compile()(items.Last())
            : null;

        return (items, nextCursor);
    }

    private static Expression<Func<T, bool>> BuildCursorExpression<T, TKey>(
        Expression<Func<T, TKey>> keySelector,
        TKey lastId)
    where TKey : struct, IComparable<TKey>
    {
        var parameter = keySelector.Parameters[0];

        var body = Expression.GreaterThan(
            keySelector.Body,
            Expression.Constant(lastId)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static async Task<(IReadOnlyList<T> Items, int TotalCount, int TotalPages)> OffsetPaginateAsync<T>(
        this IQueryable<T> query,
        int page,
        int size,
        int maxSize,
        CancellationToken cancellationToken = default)
    {
        var validPage = page < 1 ? 1 : page;
        var validSize = size < 1 ? 10 : Math.Min(size, maxSize);

        var totalCount = await query.CountAsync(cancellationToken);

        var skip = (validPage - 1) * validSize;

        var items = await query
            .Skip(skip)
            .Take(validSize)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)validSize);

        return (items, totalCount, totalPages);
    }
}