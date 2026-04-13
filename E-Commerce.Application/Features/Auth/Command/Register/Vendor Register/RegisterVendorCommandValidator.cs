using FluentValidation;

namespace E_Commerce.Application.Features.Auth.Command.Register;

internal sealed class RegisterVendorCommandValidator : AbstractValidator<RegisterVendorCommand>
{
    public RegisterVendorCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(3).WithMessage("First name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("First name must not exceed 30 characters.");

        RuleFor(u => u.MiddleName)
            .MinimumLength(3).WithMessage("Middle name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("Middle name must not exceed 30 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("Last name must not exceed 30 characters.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(U => U.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(U => U.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

        When(u => u.Image != null, () =>
        {
            RuleFor(u => u.Image!)
                .Must(image => image.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must not exceed 5MB.");

            RuleFor(u => u.Image!)
                .Must(image => image.ContentType.StartsWith("image/"))
                .WithMessage("Invalid file format. Please upload an image.");

            RuleFor(u => u.Image!)
                .Must(image =>
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Only JPG, JPEG, PNG, and WEBP formats are allowed.");
        });

        RuleFor(U => U.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of birth must be in the past.")
            // Optional Business Rule: Must be 21 or older
            .Must(BeAtLeast21).WithMessage("Vendor must be at least 21 years old.");

        RuleFor(v => v.StoreName)
            .NotEmpty().WithMessage("Store name is required.")
            .MinimumLength(3).WithMessage("Store name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Store name must not exceed 100 characters.");

        RuleFor(v => v.CommercialRegistrationNumber)
            .NotEmpty().WithMessage("Commercial Registration Number (CRN) is required.")
            .Matches(@"^\d+$").WithMessage("CRN must contain digits only.") 
            .Length(10).WithMessage("CRN must be exactly 10 digits."); 
    }

    private bool BeAtLeast21(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age)) age--;
        return age >= 21;
    }
}