using FluentValidation;

namespace E_Commerce.Application.Features.Carts.Commands.Buy_Now;

internal sealed class BuyNowCommandValidator : AbstractValidator<BuyNowCommand>
{
    public BuyNowCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Quantity)
            .GreaterThan((byte)0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo((byte)10)
            .WithMessage("Quantity must be less than or equal to 10.");
    }
}
