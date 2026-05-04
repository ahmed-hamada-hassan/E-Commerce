using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Return_Request_Order;

internal sealed class ReturnRequestOrderCommandHandler : IRequestHandler<ReturnRequestOrderCommand, Result<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReturnRequestOrderCommandHandler> _logger;

    public ReturnRequestOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ILogger<ReturnRequestOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ReturnRequestOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);
        if(order is null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for return request", request.OrderId);
            return Result<bool>.Failure(ReturnRequestErrors.OrderNotFound);
        }

        if(order.UserId != request.UserId)
        {
            _logger.LogWarning("User with ID {UserId} is not authorized to request return for Order ID {OrderId}", request.UserId, request.OrderId);
            return Result<bool>.Failure(ReturnRequestErrors.AccessDenied);
        }

        foreach(var item in request.Items)
        {
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == item.ProductId);
            if(orderItem is null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found in Order ID {OrderId} for return request", item.ProductId, request.OrderId);
                return Result<bool>.Failure(ReturnRequestErrors.OrderItemNotFound);
            }
            if(item.Quantity <= 0 || orderItem.Quantity < item.Quantity)
            {
                _logger.LogWarning("Invalid return quantity {Quantity} for Product ID {ProductId} in Order ID {OrderId}", item.Quantity, item.ProductId, request.OrderId);
                return Result<bool>.Failure(ReturnRequestErrors.InvalidQuantity);
            }

            var result = order.AddReturnRequest(item.ProductId, item.Quantity, request.Reason);
            if(result.IsFailure)
            {
                _logger.LogWarning("Failed to add return request for Product ID {ProductId} in Order ID {OrderId}: {Error}", item.ProductId, request.OrderId, result.Error);
                return Result<bool>.Failure(result.Error);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
