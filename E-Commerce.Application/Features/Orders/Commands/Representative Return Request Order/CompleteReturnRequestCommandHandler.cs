using E_Commerce.Application.Features.Orders.Commands.Refund_Order;
using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;

internal sealed class CompleteReturnRequestCommandHandler : IRequestHandler<CompleteReturnRequestCommand, Result<bool>>
{
    private readonly IReturnRequestRepository _returnRequestRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteReturnRequestCommandHandler> _logger;

    public CompleteReturnRequestCommandHandler(IReturnRequestRepository returnRequestRepository, IProductRepository productRepository, 
        IMediator mediator, IUnitOfWork unitOfWork, ILogger<CompleteReturnRequestCommandHandler> logger)
    {
        _returnRequestRepository = returnRequestRepository;
        _productRepository = productRepository;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CompleteReturnRequestCommand request, CancellationToken cancellationToken)
    {
        var returnReq = await _returnRequestRepository.GetByIdAsync(request.ReturnRequestId, cancellationToken);
        if (returnReq is null)
        {
            _logger.LogError("Return request with id {Id} not found", request.ReturnRequestId);
            return Result<bool>.Failure(ReturnRequestErrors.NotFound);
        }
        if(returnReq.Status != ReturnStatus.Approved)
        {
            _logger.LogError("Return request with id {Id} is not approved", request.ReturnRequestId);
            return Result<bool>.Failure(ReturnRequestErrors.InvalidStatus);
        }

        switch(request.Status)
        {
            case ReturnStatus.Rejected:
                returnReq.Reject(); 
                _logger.LogInformation("Return request {Id} rejected by delivery.", request.ReturnRequestId);
                break;
            case ReturnStatus.Completed:

                var refundItems = new List<ReturnRequestItemsDto> { new ReturnRequestItemsDto(returnReq.ProductId, returnReq.Quantity) };
                var refundCommand = new RefundOrderCommand(returnReq.OrderId, request.RepresentativeId, refundItems, request.Reason);
                var refundResult = await _mediator.Send(refundCommand, cancellationToken);
                if (refundResult.IsFailure)
                {
                    _logger.LogWarning("Refund failed for Return Request ID {ReturnRequestId}.", request.ReturnRequestId);
                    return Result<bool>.Failure(refundResult.Error);
                }

                foreach (var item in refundItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                    if (product is null)
                    {
                        _logger.LogWarning("Product with ID {ProductId} not found for refund.", item.ProductId);
                        return Result<bool>.Failure(ReturnRequestErrors.ItemNotFound);
                    }
                    if (item.Quantity > product.StockQuantity)
                    {
                        _logger.LogWarning("Insufficient stock to restock product with ID {ProductId}. Requested: {Requested}, Available: {Available}.",
                            item.ProductId, item.Quantity, product.StockQuantity);
                        return Result<bool>.Failure(ReturnRequestErrors.InvalidQuantity);
                    }

                    var restockResult = product.Restock(item.Quantity);
                    if (restockResult.IsFailure)
                    {
                        _logger.LogWarning("Failed to restock product with ID {ProductId}.", item.ProductId);
                        return Result<bool>.Failure(restockResult.Error);
                    }
                }
                returnReq.Complete();
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);  
    }
}
