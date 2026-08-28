using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Customer_Get_Product;

public record GetProductsByIdsQuery(List<string> Ids) : IRequest<Result<IEnumerable<CustomerProductDetailsResponse>>>;