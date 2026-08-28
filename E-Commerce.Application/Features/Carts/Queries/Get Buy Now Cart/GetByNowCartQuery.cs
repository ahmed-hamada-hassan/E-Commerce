using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Queries.Get_Buy_Now_Cart;

public record GetByNowCartQuery(Guid CartId) : IRequest<Result<BuyNowCartResponse>>;