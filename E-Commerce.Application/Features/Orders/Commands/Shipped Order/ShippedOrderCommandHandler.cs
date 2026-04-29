using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Shipped_Order;

internal sealed class ShippedOrderCommandHandler : IRequestHandler<ShippedOrderCommand, Result<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ShippedOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ShippedOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetProcessingOrderById(request.OrderId, cancellationToken);
        if(order is null)
            return Result<bool>.Failure(OrderErrors.NotFound);

        order.MarkAsShipped();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
