using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Command.UpdateProduct;

public record UpdateProductCommand(Guid VendorId, Guid ProductId, Guid CategoryId, string Name, string? Description, 
    decimal Price, string SKU, string? Barcode, int Quantity) : IRequest<Result<bool>>;