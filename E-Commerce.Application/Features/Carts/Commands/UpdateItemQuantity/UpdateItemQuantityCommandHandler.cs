using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.UpdateItemQuantity;

internal sealed class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommand, Result<bool>>
{
    private readonly IProductRepository _porductRepository;
    private readonly ICartRepository _cartRepository;

    public UpdateItemQuantityCommandHandler(IProductRepository porductRepository, ICartRepository cartRepository)
    {
        _porductRepository = porductRepository;
        _cartRepository = cartRepository;
    }

    public async Task<Result<bool>> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.UserId, cancellationToken);
        if (cart is null) return Result<bool>.Failure(CartErrors.CartNotFound);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if(item is null) return Result<bool>.Failure(CartErrors.CartItemNotFound);

        var product = await _porductRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        if(product.StockQuantity < request.Quantity) return Result<bool>.Failure(ProductErrors.InsufficientStock);

        item.Quantity = request.Quantity;

        await _cartRepository.UpdateAsync(cart, request.UserId, cancellationToken);

        return Result<bool>.Success(true);
    }
}
