using FluentValidation;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
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

        RuleFor(U => U.ImageUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(U => !string.IsNullOrEmpty(U.ImageUrl))
            .WithMessage("Image URL must be a valid URI format.");

        RuleFor(U => U.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of birth must be in the past.")
            // Optional Business Rule: Must be 18 or older
            .Must(BeAtLeast18).WithMessage("User must be at least 18 years old.");

    }

    // Optional Helper Method for the 18+ rule
    private bool BeAtLeast18(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow); 
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
