namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record VendorImageDetailsResponse(string ImageUrl, bool IsPrimary, byte DisplayOrder);