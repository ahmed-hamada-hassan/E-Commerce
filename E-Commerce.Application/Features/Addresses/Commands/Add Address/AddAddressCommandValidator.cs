using FluentValidation;

namespace E_Commerce.Application.Features.Addresses.Commands;

internal sealed class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
{
    public AddAddressCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
        RuleForEach(x => x.Addresses).ChildRules(address =>
        {
            address.RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required.");
            address.RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.");
            address.RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal Code is required.");
            address.RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.");
            address.RuleFor(x => x.AddressType)
                .IsInEnum().WithMessage("Invalid Address Type. Must be 1 (Shipping) or 2 (Billing).");
        });
    }
}
