using FluentValidation;

namespace E_Commerce.Application.Features.Orders.Commands.Place_Order;

internal sealed class PlaceOrderCommandValidate : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidate()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");

        RuleFor(x => x)
            .Must(x => (x.UseDefaulShippingAddress == true) ||
                                      x.AddressId.HasValue ||
                                      x.NewAddress is not null
                                      )
            .WithMessage("A shipping address is required to place an order. Please provide a valid address or select the default shipping address.");

        RuleFor(x => x.NewAddress!)
            .ChildRules(address =>
            {
                address.RuleFor(x => x.AddressLine1)
                    .NotEmpty().WithMessage("Address Line 1 is required.");

                address.RuleFor(x => x.City)
                    .NotEmpty().WithMessage("City is required.");

                address.RuleFor(x => x.PostalCode)
                    .NotEmpty().WithMessage("Postal Code is required.");

                address.RuleFor(x => x.Country)
                    .NotEmpty().WithMessage("Country is required.");
            })
            .When(x => x.NewAddress != null);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method. Please select a valid payment method.");
    }
}
