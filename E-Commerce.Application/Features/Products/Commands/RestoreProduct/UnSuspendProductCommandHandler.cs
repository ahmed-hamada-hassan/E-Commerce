using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Command.RestoreProduct;

internal sealed class UnSuspendProductCommandHandler : IRequestHandler<UnSuspendProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UnSuspendProductCommandHandler> _logger;

    public UnSuspendProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<UnSuspendProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UnSuspendProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAdminSuspendProductByIdAsync(request.ProductId, cancellationToken);
        if (product is null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        product.UnSuspendByAdmin();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
