using FluentValidation;

namespace E_Commerce.Application.Features.Orders.Commands.Return_Request_Order;

internal sealed class ReturnRequestOrderValidator : AbstractValidator<ReturnRequestOrderCommand>
{
    public ReturnRequestOrderValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason for return is required.")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");

        RuleFor(x => x.Items).ForEach(item =>
            item.Cascade(CascadeMode.Stop)
                .Must(i => i.ProductId != Guid.Empty).WithMessage("Product ID is required.")
                .Must(i => i.Quantity > 0).WithMessage("Quantity must be greater than zero.")
        ).NotEmpty().WithMessage("At least one item must be included for refund.");
    }
}