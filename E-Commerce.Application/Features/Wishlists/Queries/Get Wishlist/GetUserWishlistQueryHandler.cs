using E_Commerce.Application.Features.Wishlists.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Wishlists.Queries.Get_Wishlist;

internal sealed class GetUserWishlistQueryHandler : IRequestHandler<GetUserWishlistQuery, Result<WishlistResponse>>
{
    private readonly IAppDbContext _context;

    public GetUserWishlistQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<WishlistResponse>> Handle(GetUserWishlistQuery request, CancellationToken cancellationToken)
    {
        var wishlist = await _context.Wishlists
            .AsNoTracking()
            .Where(w => w.UserId == request.UserId)
            .Select(w => new WishlistResponse(
                w.Id,
                w.Items.Select(i => new WishlistItemResponse(
                    i.ProductId,
                    i.Product.Name,
                    i.Product.Price,
                    i.Product.MainImageUrl!
                )).ToList()
            )).FirstOrDefaultAsync(cancellationToken);

        if(wishlist is null)
            wishlist = new WishlistResponse(Guid.Empty, new List<WishlistItemResponse>());

        return Result<WishlistResponse>.Success(wishlist);
    }
}
