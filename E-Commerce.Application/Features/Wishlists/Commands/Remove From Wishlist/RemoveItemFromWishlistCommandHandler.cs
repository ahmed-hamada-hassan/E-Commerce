using E_Commerce.Application.Features.Wishlists.Commands.Remove_From_Wishlist;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Wishlists.Commands;

internal sealed class RemoveItemFromWishlistCommandHandler : IRequestHandler<RemoveItemFromWishlistCommand, Result<bool>>
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemFromWishlistCommandHandler(IWishlistRepository wishlistRepository, IUnitOfWork unitOfWork)
    {
        _wishlistRepository = wishlistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(RemoveItemFromWishlistCommand request, CancellationToken cancellationToken)
    {
        var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(request.UserId, cancellationToken);
        if(wishlist is null)
            return Result<bool>.Failure(WishlistErrors.WishlistNotFound);

        var removeResult = wishlist.RemoveItem(request.ProductId);
        if(removeResult.IsFailure)
            return Result<bool>.Failure(removeResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
