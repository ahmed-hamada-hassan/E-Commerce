using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record ImageResponse(IFormFile Image, bool IsPrimary, byte DisplayOrder);