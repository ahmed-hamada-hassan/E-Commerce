using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Commands.Admin_Return_Request_Order;

internal sealed class AdminReturnRequestOrderCommandHandler : IRequestHandler<AdminReturnRequestOrderCommand, Result<bool>>
{
    private readonly IReturnRequestRepository _returnRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminReturnRequestOrderCommandHandler> _logger;

    public AdminReturnRequestOrderCommandHandler(IReturnRequestRepository returnRequestRepository, IUnitOfWork unitOfWork,
        ILogger<AdminReturnRequestOrderCommandHandler> logger)
    {
        _returnRequestRepository = returnRequestRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AdminReturnRequestOrderCommand request, CancellationToken cancellationToken)
    {
        var returnReq = await _returnRequestRepository.GetByIdAsync(request.ReturnRequestId, cancellationToken);
        if (returnReq is null)
        {
            _logger.LogError("Return request with id {Id} not found", request.ReturnRequestId);
            return Result<bool>.Failure(ReturnRequestErrors.NotFound);
        }

        switch (request.Status)
        {
            case ReturnStatus.Approved:
                returnReq.Approve();
                break;
            case ReturnStatus.Rejected:
                returnReq.Reject();
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
