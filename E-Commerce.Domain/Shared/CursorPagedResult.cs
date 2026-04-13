namespace E_Commerce.Domain.Shared;

public class CursorPagedResult<T, TCursor> 
    where T : class
    where TCursor : struct
{
    public IReadOnlyCollection<T> Items { get; }
    public TCursor? NextCursor { get; }

    public CursorPagedResult(IReadOnlyCollection<T> items, TCursor? nextCursor)
    {
        Items = items;
        NextCursor = nextCursor;
    }

    public bool HasNextPage => NextCursor.HasValue;
}