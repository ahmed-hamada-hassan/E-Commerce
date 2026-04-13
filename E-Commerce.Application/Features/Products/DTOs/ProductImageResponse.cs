namespace E_Commerce.Application.Features.Products.DTOs;

public record ProductImageResponse(string ImageUrl, bool IsPrimary, byte DisplayOrder);