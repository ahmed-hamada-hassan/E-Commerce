namespace E_Commerce.Application.Features.Products.DTOs;

public record VendorArchivedProductResponse(Guid ProductId, Guid CategoryId, string CategoryName, string Name, string? Description, decimal Price,
    string SKU, string? Barcode, int Quantity, List<ProductImageResponse> Images, bool IsDeleted, DateTimeOffset? DeletedOn);