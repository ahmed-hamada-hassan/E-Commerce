    using FluentValidation;

namespace E_Commerce.Application.Features.Wishlists.Commands.Add_To_Wishlist;

internal sealed class AddItemToWishlistCommandValidator : AbstractValidator<AddItemToWishlistCommand>
{
    public AddItemToWishlistCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}
