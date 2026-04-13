namespace E_Commerce.Domain.Shared;

public class OffsetPagedResult<T> where T : class
{
    public IReadOnlyCollection<T> Items { get; }
    public int Page { get; }
    public int Size { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public OffsetPagedResult(IReadOnlyCollection<T> items, int page, int size, int totalCount, int totalPages)
    {
        Items = items;
        Page = page;
        Size = size;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public IEnumerable<int> GetVisiblePages(int maxVisible = 5)
    {
        var startPage = Math.Max(1, Page - maxVisible / 2);

        var endPage = Math.Min(TotalPages, startPage + maxVisible - 1);

        if (endPage - startPage + 1 < maxVisible)
        {
            startPage = Math.Max(1, endPage - maxVisible + 1);
        }

        return Enumerable.Range(startPage, endPage - startPage + 1);
    }
}