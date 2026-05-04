using E_Commerce.Domain.Enums;
using FluentValidation;

namespace E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;

internal sealed class CompleteReturnRequestCommandValidator : AbstractValidator<CompleteReturnRequestCommand>
{
    public CompleteReturnRequestCommandValidator()
    {
        RuleFor(c => c.ReturnRequestId)
            .NotEmpty().WithMessage("Return Request ID is required.");

        RuleFor(c => c.RepresentativeId)
            .NotEmpty().WithMessage("Representative ID is required.");

        RuleFor(c => c.Status)
            .Must(status => status == ReturnStatus.Rejected || status == ReturnStatus.Completed)
            .IsInEnum().WithMessage("Invalid return status.");

        RuleFor(c => c.Reason)
            .NotEmpty().WithMessage("Reason for return request is required.")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
    }
}
