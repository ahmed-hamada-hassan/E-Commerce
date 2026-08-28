using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.RemoveItem;

internal sealed class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, Result<CartSummaryResponse>>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Result<CartSummaryResponse>> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.CartId, cancellationToken);

        if (cart is null) return Result<CartSummaryResponse>.Failure(CartErrors.CartNotFound);

        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (itemToRemove is null) return Result<CartSummaryResponse>.Failure(CartErrors.CartItemNotFound);

        cart.Items.Remove(itemToRemove);
        if (cart.Items.Any()) await _cartRepository.UpdateAsync(cart, cancellationToken);
        else await _cartRepository.DeleteAsync(request.CartId, cancellationToken);

        return Result<CartSummaryResponse>.Success(cart.ToCartSummaryResponse());
    }
}
