using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record ImageRequest(IFormFile Image, bool IsPrimary, byte DisplayOrder);