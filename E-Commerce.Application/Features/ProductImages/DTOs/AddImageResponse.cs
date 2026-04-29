namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record AddImageResponse(Guid ImageId, string Url, byte DisplayOrder, bool IsPrimary);