using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Place_Order;

internal sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<Guid>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    public PlaceOrderCommandHandler(ICartRepository cartRepository, IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IProductRepository productRepository, ILogger<PlaceOrderCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetAsync(request.UserId, cancellationToken);
        if(cart is null || !cart.Items.Any())
        {
            _logger.LogWarning("User with id {UserId} attempted to place an order with an empty cart.", request.UserId);
            return Result<Guid>.Failure(CartErrors.CartNotFound);
        }

        var user = await _userManager.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if(user is null)
        {
            _logger.LogWarning("User with id {UserId} not found.", request.UserId);
            return Result<Guid>.Failure(ApplicationUserErrors.NotFound);
        }

        Address? selectedAddress = null;

        if(request.NewAddress is not null)
        {
            var newAddress = Address.Create(request.UserId, request.NewAddress.AddressLine1, request.NewAddress.AddressLine2, 
                request.NewAddress.City, request.NewAddress.StateOrProvince, request.NewAddress.PostalCode, 
                request.NewAddress.Country, AddressType.Shipping);
            if(newAddress.IsFailure)
            {
                _logger.LogWarning("Failed to create new address for user with id {UserId}. Errors: {Errors}", request.UserId, newAddress.Error);
                return Result<Guid>.Failure(newAddress.Error);
            }

            selectedAddress = newAddress.Value;

            user.AddAddress(selectedAddress!);
        }
        else if(request.AddressId is not null)
        {
            selectedAddress = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId && !a.IsDeleted);
        }
        else if(request.UseDefaulShippingAddress.HasValue)
            selectedAddress = user.DefaultShippingAddress;

        if(selectedAddress is null)
        {
            _logger.LogWarning("User with id {UserId} attempted to place an order without providing an address.", request.UserId);
            return Result<Guid>.Failure(OrderErrors.AddressRequired);
        }

        var addressSnapshot = $"{selectedAddress.AddressLine1}, {(string.IsNullOrEmpty(selectedAddress.AddressLine2) ? "" : selectedAddress.AddressLine2 + ", ")}" +
            $"{selectedAddress.City}, {(string.IsNullOrEmpty(selectedAddress.StateOrProvince) ? "" : selectedAddress.StateOrProvince + ", ")}" +
            $"{selectedAddress.PostalCode}, {selectedAddress.Country}";

        var shippingCost = 40.00m; // This could be calculated based on the address and order details

        var order = Order.Create(request.UserId, selectedAddress.Id, addressSnapshot, shippingCost);
        if(order.IsFailure)
        {
            _logger.LogWarning("Failed to create order for user with id {UserId}. Errors: {Errors}", request.UserId, order.Error);
            return Result<Guid>.Failure(order.Error);
        }

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if(product is null)
            {
                _logger.LogWarning("Product with id {ProductId} not found for user with id {UserId}.", item.ProductId, request.UserId);
                return Result<Guid>.Failure(ProductErrors.ProductNotFound);
            }

            if(product.StockQuantity < item.Quantity)
            {
                _logger.LogWarning("Insufficient stock for product with id {ProductId} for user with id {UserId}.", item.ProductId, request.UserId);
                return Result<Guid>.Failure(ProductErrors.InsufficientStock);
            }

            var result = product.DeductStock(item.Quantity);
            if(result.IsFailure)
            {
                _logger.LogWarning("Failed to deduct stock for product with id {ProductId} for user with id {UserId}. Errors: {Errors}", item.ProductId, request.UserId, result.Error);
                return Result<Guid>.Failure(result.Error);
            }

            order.Value!.AddOrderItem(product.Id, product.Name, product.Price, item.Quantity);
        }

        var orderId = await _orderRepository.AddAsync(order.Value!, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cartRepository.DeleteAsync(user.Id, cancellationToken);

        return Result<Guid>.Success(orderId);
    }
}