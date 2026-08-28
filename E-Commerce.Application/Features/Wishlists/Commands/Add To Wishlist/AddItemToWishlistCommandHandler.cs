using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Wishlists.Commands.Add_To_Wishlist;

internal sealed class AddItemToWishlistCommandHandler : IRequestHandler<AddItemToWishlistCommand, Result<Guid>>
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToWishlistCommandHandler(IWishlistRepository wishlistRepository, IUnitOfWork unitOfWork)
    {
        _wishlistRepository = wishlistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddItemToWishlistCommand request, CancellationToken cancellationToken)
    {
        var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(request.UserId, cancellationToken);
        var isNewWishlist = false;

        if(wishlist is null)
        {
            var createWishlistResult = Wishlist.Create(request.UserId);
            if (createWishlistResult.IsFailure)
                return Result<Guid>.Failure(createWishlistResult.Error);

            wishlist = createWishlistResult.Value!;

            isNewWishlist = true;
        }

        var addItemResult = wishlist.AddItem(request.ProductId);
        if (addItemResult.IsFailure)
            return Result<Guid>.Failure(addItemResult.Error);

        if (isNewWishlist)
            await _wishlistRepository.AddWishlistAsync(wishlist, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(wishlist.Id);
    }
}
