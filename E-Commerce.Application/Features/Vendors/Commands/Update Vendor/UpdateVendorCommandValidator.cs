using FluentValidation;

namespace E_Commerce.Application.Features.Vendors.Commands.Update_Vendor;

internal sealed class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor ID is required.");

        RuleFor(x => x.StoreName)
            .MinimumLength(3).WithMessage("Store name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Store name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.StoreName));

        RuleFor(x => x.CommercialRegistrationNumber)
            .Length(10).WithMessage("Commercial registration number must be exactly 10 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CommercialRegistrationNumber));
    }
}
