using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Refund_Order;

internal sealed class RefundOrderCommandHandler : IRequestHandler<RefundOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentFactory _paymentFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefundOrderCommandHandler> _logger;

    public RefundOrderCommandHandler(IOrderRepository orderRepository, IPaymentFactory paymentFactory, IUnitOfWork unitOfWork, 
        ILogger<RefundOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _paymentFactory = paymentFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(RefundOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);
        if(order is null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for refund.", request.OrderId);
            return Result<Guid>.Failure(RefundErrors.OrderNotFound);
        }

        var paymentService = _paymentFactory.GetPaymentService(order.Payment!.PaymentMethod);
        var refundResult = await paymentService.RefundPaymentAsync(request.AdminId, order, request.Items, request.Reason, cancellationToken);
        if(refundResult.IsFailure)
        {
            _logger.LogWarning("Refund failed for Order ID {OrderId}.", request.OrderId);
            return Result<Guid>.Failure(refundResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(refundResult.Value);
    }
}
