using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Command.DeleteProduct;

internal sealed class SuspendProductCommandHandler : IRequestHandler<SuspendProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuspendProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(SuspendProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAdminProductByIdAsync(request.ProductId, cancellationToken);
        if (product == null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        product.SuspendByAdmin();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
