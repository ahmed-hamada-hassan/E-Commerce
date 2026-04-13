namespace E_Commerce.Application.Features.Products.DTOs;

public record CustomerProductResponse(string categoryName, string name, string? description, decimal price, string sku, string? barcode, List<ProductImageResponse> images);