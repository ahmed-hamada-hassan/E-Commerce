namespace E_Commerce.Application.Features.Products.DTOs;

public record CustomerProductsRequest(string? SearchTerm, decimal? MinPrice, decimal? MaxPrice, string? SortBy,
    int Page = 1, int Size = 10);