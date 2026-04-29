using FluentValidation;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .MinimumLength(3).WithMessage("First name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("First name must not exceed 30 characters.")
            .When(u => !string.IsNullOrEmpty(u.FirstName));

        RuleFor(u => u.MiddleName)
            .MinimumLength(3).WithMessage("Middle name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("Middle name must not exceed 30 characters.")
            .When(u => !string.IsNullOrEmpty(u.MiddleName));

        RuleFor(u => u.LastName)
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters.")
            .MaximumLength(30).WithMessage("Last name must not exceed 30 characters.")
            .When(u => !string.IsNullOrEmpty(u.LastName));

        RuleFor(u => u.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .When(u => !string.IsNullOrEmpty(u.Email));

        RuleFor(U => U.UserName)
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .When(u => !string.IsNullOrEmpty(u.UserName));

        RuleFor(U => U.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
            .When(u => !string.IsNullOrEmpty(u.PhoneNumber));

        RuleFor(U => U.DateOfBirth)
            .Must(dob => BeAtLeast18(dob!.Value))
            .When(u => u.DateOfBirth.HasValue)
            .WithMessage("User must be at least 18 years old.");
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
