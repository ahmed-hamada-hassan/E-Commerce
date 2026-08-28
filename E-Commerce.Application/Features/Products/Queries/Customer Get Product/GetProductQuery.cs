using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IRequest<Result<CustomerProductDetailsResponse>>;
