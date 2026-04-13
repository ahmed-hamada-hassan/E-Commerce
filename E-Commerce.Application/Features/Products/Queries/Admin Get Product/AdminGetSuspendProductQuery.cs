using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

public record AdminGetSuspendProductQuery(Guid ProductId) : IRequest<Result<AdminSuspendProductResponse>>;