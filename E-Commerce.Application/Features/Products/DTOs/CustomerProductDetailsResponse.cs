namespace E_Commerce.Application.Features.Products.DTOs;

public record CustomerProductDetailsResponse(
    Guid Id,
    string categoryName,
    Guid categoryId,
    string name,
    string? description,
    decimal price,
    string sku,
    string? barcode,
    bool isInStock,
    byte? lowStockQuantity,
    byte maxAllowedPerOrder,
    double AverageRating,
    int TotalReviews,
    List<ProductImageResponse> images
);