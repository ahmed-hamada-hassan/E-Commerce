using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetArchivedProductsQueryHandler :
    IRequestHandler<AdminGetArchivedProductsQuery, Result<CursorPagedResult<AdminArchivedProductResponse, Guid>>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetArchivedProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CursorPagedResult<AdminArchivedProductResponse, Guid>>> Handle(AdminGetArchivedProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAdminArchivedProductsAsync(request.VendorId, request.Cursor,
            request.Size, cancellationToken);

        var pagedResponse = new CursorPagedResult<AdminArchivedProductResponse, Guid>(
            products.Items.Select(p => p.ToAdminArchivedProductResponse()).ToList(), products.NextCursor);

        return Result<CursorPagedResult<AdminArchivedProductResponse, Guid>>.Success(pagedResponse);
    }
}