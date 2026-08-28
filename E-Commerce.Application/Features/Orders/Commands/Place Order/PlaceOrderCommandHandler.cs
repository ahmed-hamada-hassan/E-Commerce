using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
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
    private readonly IPaymentFactory _paymentFactory;

    public PlaceOrderCommandHandler(ICartRepository cartRepository, IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IProductRepository productRepository, 
        ILogger<PlaceOrderCommandHandler> logger, IPaymentFactory paymentFactory)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _paymentFactory = paymentFactory;
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = request.CartId.HasValue
            ? await _cartRepository.GetBuyNowCartAsync(request.CartId.Value, cancellationToken):
              await _cartRepository.GetAsync(request.UserId, cancellationToken);

        if (cart is null || !cart.Items.Any())
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
            if(user.Addresses.Count >= 5)
            {
                _logger.LogWarning("User with id {UserId} attempted to add a new address but has reached the maximum limit of 5 active addresses.", request.UserId);
                return Result<Guid>.Failure(AddressErrors.MaxActiveAddressesReached);
            }

            var newAddress = Address.Create(request.UserId, request.NewAddress.AddressLine1, request.NewAddress.AddressLine2, 
                request.NewAddress.City, request.NewAddress.StateOrProvince, request.NewAddress.PostalCode, 
                request.NewAddress.Country, AddressType.Shipping);
            if(newAddress.IsFailure)
            {
                _logger.LogWarning("Failed to create new address for user with id {UserId}. Errors: {Errors}", request.UserId, newAddress.Error);
                return Result<Guid>.Failure(newAddress.Error);
            }

            selectedAddress = newAddress.Value;

            var addAddressResult = user.AddAddress(selectedAddress!);
            if(addAddressResult.IsFailure)
            {
                _logger.LogWarning("Failed to add address for user with id {UserId}. Errors: {Errors}", request.UserId, addAddressResult.Error);
                return Result<Guid>.Failure(addAddressResult.Error);
            }
        }
        else if(request.AddressId is not null)
        {
            selectedAddress = user.Addresses.FirstOrDefault(a => a.Id == request.AddressId);
        }
        else if(request.UseDefaultShippingAddress.HasValue && request.UseDefaultShippingAddress.Value)
            selectedAddress = user.DefaultShippingAddress;

        if(selectedAddress is null)
        {
            _logger.LogWarning("User with id {UserId} attempted to place an order without providing an address.", request.UserId);
            return Result<Guid>.Failure(OrderErrors.AddressRequired);
        }

        var addressSnapshot = $"{selectedAddress.AddressLine1}, {(string.IsNullOrEmpty(selectedAddress.AddressLine2) ? "" : selectedAddress.AddressLine2 + ", ")}" +
            $"{selectedAddress.City}, {(string.IsNullOrEmpty(selectedAddress.StateOrProvince) ? "" : selectedAddress.StateOrProvince + ", ")}" +
            $"{selectedAddress.PostalCode}, {selectedAddress.Country}";

        var shippingCost = 50.00m; // This could be calculated based on the address and order details

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

            order.Value!.AddOrderItem(product.Id, product.Name, product.MainImageUrl!, product.Price, item.Quantity);
        }

        var paymentService = _paymentFactory.GetPaymentService(request.PaymentMethod);
        if(paymentService is null)
        {
            _logger.LogWarning("Unsupported payment method {PaymentMethod} for user with id {UserId}.", request.PaymentMethod, request.UserId);
            return Result<Guid>.Failure(PaymentErrors.UnsupportedPaymentMethod);
        }

        var paymentResult = await paymentService.ProcessPaymentAsync(order.Value!, order.Value!.TotalAmount, cancellationToken);

        if(paymentResult.IsFailure)
        {
            _logger.LogWarning("Failed to process payment for user {UserId}. Error: {Error}", request.UserId, paymentResult.Error); 
            return Result<Guid>.Failure(paymentResult.Error);
        }

        var addPaymentResult = order.Value!.AddPayment(request.PaymentMethod);

        if (addPaymentResult.IsFailure)
        {
            _logger.LogWarning("Failed to add payment for user {UserId}. Error: {Error}", request.UserId, addPaymentResult.Error);
            return Result<Guid>.Failure(addPaymentResult.Error);
        }

        var orderId = await _orderRepository.AddAsync(order.Value!, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if(request.CartId.HasValue)
            await _cartRepository.DeleteBuyNowCartAsync(request.CartId.Value, cancellationToken);
        else
            await _cartRepository.DeleteAsync(user.Id, cancellationToken);

        return Result<Guid>.Success(orderId);
    }
}