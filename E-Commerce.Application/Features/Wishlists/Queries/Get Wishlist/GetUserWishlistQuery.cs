using E_Commerce.Application.Features.Wishlists.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Wishlists.Queries.Get_Wishlist;

public record GetUserWishlistQuery(Guid UserId) : IRequest<Result<WishlistResponse>>;