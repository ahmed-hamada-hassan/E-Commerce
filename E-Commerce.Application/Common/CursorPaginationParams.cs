namespace E_Commerce.Application.Common;

public record CursorPaginationParams<TKey>(TKey? cursor = default, int size = 10);
