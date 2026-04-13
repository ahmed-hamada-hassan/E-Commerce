namespace E_Commerce.Application.Features.Products.DTOs;

public record VendorProductResponse(Guid ProductId, Guid CategoryId, string CategoryName, string Name, string? Description, decimal Price,
    string SKU, string? Barcode, int Quantity, List<ProductImageResponse> Images);