namespace E_Commerce.Application.Features.Wishlists.DTOs;

public record WishlistItemResponse(Guid ProductId, string ProductName, decimal ProductPrice, string MainImageUrl);