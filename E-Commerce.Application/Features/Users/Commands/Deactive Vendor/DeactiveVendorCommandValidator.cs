using FluentValidation;

namespace E_Commerce.Application.Features.Users.Commands.Deactive_Vendor;

internal sealed class DeactiveVendorCommandValidator : AbstractValidator<DeactiveVendorCommand>
{
    public DeactiveVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor Id is required.");
    }
}
