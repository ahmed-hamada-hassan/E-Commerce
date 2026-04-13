using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

public record VendorGetProductQuery(Guid ProductId, Guid VendorId) : IRequest<Result<VendorProductResponse>>;