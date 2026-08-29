using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Queries.GetCart;

internal sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartResponse>>
{
    private readonly ICartRepository _cartRepository;

    public GetCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.CartId, cancellationToken);
        return Result<CartResponse>.Success(cart?.ToCartResponse() ?? cart.ToEmptyCartResponse());
    }
}
