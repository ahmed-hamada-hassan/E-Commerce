using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;
using System.Globalization;

namespace E_Commerce.Application.Features.Products.Command.CreateProduct;

public record CreateProductCommand(Guid VendorId, string Name, Guid CategoryId, string? Description,
    decimal Price, string SKU, string? Barcode, int StockQuantity) : IRequest<Result<Guid>>;
