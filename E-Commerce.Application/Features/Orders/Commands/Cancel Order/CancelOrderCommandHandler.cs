using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Cancel_Order;

internal sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelOrderCommandHandler> _logger;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, ICancellationRepository cancellationRepository,
        IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<CancelOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for cancellation.", request.OrderId);
            return Result<Guid>.Failure(OrderErrors.NotFound);
        }

        var cancelResult = order.Cancel(request.UserId, request.Reason);
        if (cancelResult.IsFailure)
        {
            _logger.LogError("Failed to cancel order with ID {OrderId}.", request.OrderId);
            return Result<Guid>.Failure(cancelResult.Error);
        }

        foreach (var item in order.OrderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null)
            {
                _logger.LogError("Product with ID {ProductId} not found while cancelling order with ID {OrderId}.", item.ProductId, request.OrderId);
                return Result<Guid>.Failure(ProductErrors.ProductNotFound);
            }

            product.AddStock(item.Quantity);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(order.Cancellation!.Id);
    }

}
