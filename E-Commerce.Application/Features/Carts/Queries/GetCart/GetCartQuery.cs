using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Queries.GetCart;

public record GetCartQuery () : IRequest<Result<CartResponse>>;