using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.RemoveItem;

internal sealed class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserContext _userContext;

    public RemoveItemCommandHandler(ICartRepository cartRepository, IUserContext userContext)
    {
        _cartRepository = cartRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var cart = await _cartRepository.GetAsync(userId, cancellationToken);

        if(cart is null) return Result<bool>.Failure(CartErrors.CartNotFound);

        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if(itemToRemove is null) return Result<bool>.Failure(CartErrors.CartItemNotFound);

        cart.Items.Remove(itemToRemove);
        if(cart.Items.Any()) await _cartRepository.UpdateAsync(cart, cancellationToken);
        else await _cartRepository.DeleteAsync(userId, cancellationToken);

        return Result<bool>.Success(true);
    }
}
