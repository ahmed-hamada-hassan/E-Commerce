using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Command.CreateProduct;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork, IVendorRepository vendorRepository, ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _vendorRepository = vendorRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // This way will be more efficient in case the vendor is deactivated after the user logged in
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor is null) return Result<Guid>.Failure(VendorErrors.NotFound);

        if (!vendor.IsActive)
        {
            _logger.LogWarning("Vendor with ID {VendorId} attempted to create a product but is not active.", request.VendorId);
            return Result<Guid>.Failure(VendorErrors.NotActive);
        }

        var categoryExists = await _categoryRepository.IsExistsAsync(request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            _logger.LogWarning("Attempted to create a product with non-existent category ID {CategoryId}.", request.CategoryId);
            return Result<Guid>.Failure(CategoryErrors.NotFound);
        }

        var productResult = Product.Create(request.CategoryId, vendor.Id, request.Name, request.Description, request.Price, request.SKU,
            request.Barcode, request.StockQuantity);
        if (productResult.IsFailure) return Result<Guid>.Failure(productResult.Error);

        var product = productResult.Value!;

        await _productRepository.AddAsync(product, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
