using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record ImageDTO(IFormFile Image, bool IsPrimary, byte DisplayOrder);