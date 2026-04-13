namespace E_Commerce.Application.Features.Products.DTOs;

public record AdminProductResponse(Guid ProductId, Guid CategoryId, Guid VendorId, string CategoryName,
    string Name, string? Description, decimal Price, string SKU, string? Barcode, int Quantity, string? PrimaryImageURL);