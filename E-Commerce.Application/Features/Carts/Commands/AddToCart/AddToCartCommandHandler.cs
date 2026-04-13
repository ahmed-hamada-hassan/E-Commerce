using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.AddToCart;

internal sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserContext _userContext;

    public AddToCartCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, IUserContext userContext)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        var cart = await _cartRepository.GetAsync(userId, cancellationToken) ?? new Cart(userId);

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        var proposedQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

        if (product.StockQuantity < proposedQuantity) return Result<bool>.Failure(ProductErrors.InsufficientStock);

        if (existingItem is not null) existingItem.Quantity = proposedQuantity;
        else
            cart.Items.Add(CartItem.Create(request.ProductId, product.Name, product.Price, request.Quantity, product.MainImageUrl));

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        return Result<bool>.Success(true);
    }
}
