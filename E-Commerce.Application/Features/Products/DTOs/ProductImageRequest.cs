using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Products.DTOs;

public record ProductImageRequest(IFormFile Image, bool IsPrimary, byte DisplayOrder);
