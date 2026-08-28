using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.AddToCart;

internal sealed class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<CartSummaryResponse>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public AddToCartCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<CartSummaryResponse>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<CartSummaryResponse>.Failure(ProductErrors.ProductNotFound);

        var cart = await _cartRepository.GetAsync(request.CartId, cancellationToken) ?? new Cart(request.CartId, null);

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        var proposedQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

        if (product.StockQuantity < proposedQuantity) return Result<CartSummaryResponse>.Failure(ProductErrors.InsufficientStock);

        if (existingItem is not null) existingItem.Quantity = proposedQuantity;
        else
            cart.Items.Add(CartItem.Create(request.ProductId, product.Name, product.Price, request.Quantity, product.MainImageUrl));

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        return Result<CartSummaryResponse>.Success(cart.ToCartSummaryResponse());
    }
}