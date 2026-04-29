using FluentValidation;

namespace E_Commerce.Application.Features.Vendors.Commands.Active_Vendor;

internal sealed class ActiveVendorCommandValidator : AbstractValidator<ActiveVendorCommand>
{
    public ActiveVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("VendorId is required.");
    }
}
