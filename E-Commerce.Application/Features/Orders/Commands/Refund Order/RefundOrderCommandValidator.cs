using FluentValidation;

namespace E_Commerce.Application.Features.Orders.Commands.Refund_Order;

internal class RefundOrderCommandValidator : AbstractValidator<RefundOrderCommand>
{
    public RefundOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");

        RuleFor(x => x.AdminId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Items).ForEach(item => 
            item.Cascade(CascadeMode.Stop)
                .Must(i => i.ProductId != Guid.Empty).WithMessage("Product ID is required.")
                .Must(i => i.Quantity > 0).WithMessage("Quantity must be greater than zero.")
        ).NotEmpty().WithMessage("At least one item must be included for refund.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason for refund is required.")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
    }
}
