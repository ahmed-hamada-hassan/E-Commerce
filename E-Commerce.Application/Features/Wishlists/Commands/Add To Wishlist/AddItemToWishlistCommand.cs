using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Wishlists.Commands.Add_To_Wishlist;

public record AddItemToWishlistCommand(Guid UserId, Guid ProductId) : IRequest<Result<Guid>>;