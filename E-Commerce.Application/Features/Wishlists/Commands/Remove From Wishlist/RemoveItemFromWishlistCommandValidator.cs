using FluentValidation;

namespace E_Commerce.Application.Features.Wishlists.Commands.Remove_From_Wishlist;

internal sealed class RemoveItemFromWishlistCommandValidator : AbstractValidator<RemoveItemFromWishlistCommand>
{
    public RemoveItemFromWishlistCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}
