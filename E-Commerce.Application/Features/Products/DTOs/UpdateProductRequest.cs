namespace E_Commerce.Application.Features.Products.DTOs;

public record UpdateProductRequest(Guid CategoryId, string Name, string? Description,
    decimal Price, string SKU, string? Barcode, int Quantity);