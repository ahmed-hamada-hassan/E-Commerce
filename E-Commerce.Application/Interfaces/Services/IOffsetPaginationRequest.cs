namespace E_Commerce.Application.Interfaces.Services;

public interface IOffsetPaginationRequest
{
    int Page { get; init; }
    int Size { get; init; }
}
