using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Wishlists.Commands.Remove_From_Wishlist;

public record RemoveItemFromWishlistCommand(Guid UserId, Guid ProductId) : IRequest<Result<bool>>;