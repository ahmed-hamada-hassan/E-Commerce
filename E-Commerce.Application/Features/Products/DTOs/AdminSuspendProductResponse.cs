namespace E_Commerce.Application.Features.Products.DTOs;

public record AdminSuspendProductResponse(Guid ProductId, Guid VendorId, Guid CategoryId, string CategoryName,
    string Name, string? Description, decimal Price, string SKU, string? Barcode, int Quantity, 
    string? PrimaryImageURL, bool IsDeleted, bool IsDeletedByAdmin, DateTimeOffset? DeletedOn);