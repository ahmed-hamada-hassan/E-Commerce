namespace E_Commerce.Application.Features.Wishlists.DTOs;

public record WishlistResponse(Guid WishlistId, List<WishlistItemResponse> Items);
