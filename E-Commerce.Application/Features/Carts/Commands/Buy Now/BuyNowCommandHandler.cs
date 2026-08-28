using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.Buy_Now;

internal sealed class BuyNowCommandHandler : IRequestHandler<BuyNowCommand, Result<Guid>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public BuyNowCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(BuyNowCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<Guid>.Failure(ProductErrors.ProductNotFound);

        if (product.StockQuantity < request.Quantity) return Result<Guid>.Failure(ProductErrors.InsufficientStock);

        var uniqueBuyNowCartId = Guid.NewGuid();

        var cart = new Cart(uniqueBuyNowCartId, null);

        cart.Items.Add(CartItem.Create(request.ProductId, product.Name, product.Price, request.Quantity, product.MainImageUrl));

        await _cartRepository.SetBuyNowCartAsync(cart, cancellationToken);

        return Result<Guid>.Success(cart.Id);
    }
}
