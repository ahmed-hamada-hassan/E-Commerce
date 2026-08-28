using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Queries.Get_Buy_Now_Cart;

internal sealed class GetByNowCartQueryHandler : IRequestHandler<GetByNowCartQuery, Result<BuyNowCartResponse>>
{
    private readonly ICartRepository _cartRepository;
    public GetByNowCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<Result<BuyNowCartResponse>> Handle(GetByNowCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetBuyNowCartAsync(request.CartId, cancellationToken);
        if (cart is null) return Result<BuyNowCartResponse>.Failure(CartErrors.CartNotFound);
        return Result<BuyNowCartResponse>.Success(cart.ToBuyNowCartResponse());
    }
}
