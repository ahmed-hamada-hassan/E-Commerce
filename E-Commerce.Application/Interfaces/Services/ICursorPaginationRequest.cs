namespace E_Commerce.Application.Interfaces.Services;

public interface ICursorPaginationRequest<TCursor> where TCursor : struct
{
    TCursor? Cursor { get; init; }
    int Size { get; init; }
}