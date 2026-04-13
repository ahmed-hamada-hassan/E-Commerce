using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(string? SearchTerm, decimal? MinPrice, decimal? MaxPrice, string? SortBy,
    int Page, int Size) : IRequest<Result<OffsetPagedResult<CustomerProductResponse>>>, IOffsetPaginationRequest;