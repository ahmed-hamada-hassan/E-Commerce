using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Products.Queries.Customer_Get_Product;

internal sealed class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, Result<IEnumerable<CustomerProductDetailsResponse>>>
{
    private readonly IAppDbContext _context;

    public GetProductsByIdsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<CustomerProductDetailsResponse>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        if (request.Ids.Any())
        {
            var parsedIds = request.Ids.Select(Guid.Parse).ToList();

            var query = _context.Products
                .AsNoTracking()
                .Where(p => parsedIds.Contains(p.Id))
                .Select(p => new CustomerProductDetailsResponse
                (
                    p.Id,
                    p.Category.Name,
                    p.CategoryId,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.SKU,
                    p.Barcode,
                    p.StockQuantity > 0,
                    p.StockQuantity <= 5 ? (byte)p.StockQuantity : null,
                    (byte)Math.Min(10, p.StockQuantity),
                    Math.Round(_context.Reviews.Where(r => r.ProductId == p.Id).Average(r => (double?)r.Rating) ?? 0.0, 1),
                    _context.Reviews.Count(r => r.ProductId == p.Id),
                    p.Images.Select(pi => new ProductImageResponse(pi.ImageUrl, pi.IsPrimary, pi.DisplayOrder)).ToList()
                ));

            var products = await query.ToListAsync(cancellationToken);

            return Result<IEnumerable<CustomerProductDetailsResponse>>.Success(products);
        }

        return Result<IEnumerable<CustomerProductDetailsResponse>>.Success(Enumerable.Empty<CustomerProductDetailsResponse>());
    }
}
