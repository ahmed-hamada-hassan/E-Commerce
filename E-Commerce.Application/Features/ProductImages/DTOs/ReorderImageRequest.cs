namespace E_Commerce.Application.Features.ProductImages.DTOs;

public record ReorderImageRequest(Guid imageId, byte displayOrder);