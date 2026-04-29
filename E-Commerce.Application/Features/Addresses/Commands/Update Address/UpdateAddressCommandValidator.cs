using FluentValidation;

namespace E_Commerce.Application.Features.Addresses.Commands.Update_Address;

internal sealed class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.AddressId)
            .NotEmpty().WithMessage("Address ID is required.");

        RuleFor(x => x.AddressInfo).ChildRules(address =>
        {
            address.RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.AddressLine1));

            address.RuleFor(x => x.AddressLine2)
                .NotEmpty().WithMessage("Address Line 2 is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));

            address.RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.City));

            address.RuleFor(x => x.StateOrProvince)
                .NotEmpty().WithMessage("State or Province is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.StateOrProvince));

            address.RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal Code is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));

            address.RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.Country));

            address.RuleFor(x => x.AddressType)
                .IsInEnum().WithMessage("Invalid Address Type. Must be 1 (Shipping) or 2 (Billing).")
                .When(x => x.AddressType != default);
        });
    }
}
