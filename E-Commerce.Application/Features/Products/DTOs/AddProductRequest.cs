namespace E_Commerce.Application.Features.Products.DTOs;

public record AddProductRequest(string Name, Guid CategoryId, string? Description,
    decimal Price, string SKU, string? Barcode, int StockQuantity, List<ProductImageRequest> Images);