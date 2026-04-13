using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Queries.GetCart;

internal sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartResponse>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserContext _userContext;

    public GetCartQueryHandler(ICartRepository cartRepository, IUserContext userContext)
    {
        _cartRepository = cartRepository;
        _userContext = userContext;
    }

    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var cart = await _cartRepository.GetAsync(userId, cancellationToken) ?? new Cart(userId);

        return Result<CartResponse>.Success(cart.ToCartResponse());
    }
}
