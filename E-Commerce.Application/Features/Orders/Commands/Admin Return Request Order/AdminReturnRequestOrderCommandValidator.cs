using E_Commerce.Domain.Enums;
using FluentValidation;

namespace E_Commerce.Application.Features.Orders.Commands.Admin_Return_Request_Order;

internal sealed class AdminReturnRequestOrderCommandValidator : AbstractValidator<AdminReturnRequestOrderCommand>
{
    public AdminReturnRequestOrderCommandValidator()
    {
        RuleFor(c => c.ReturnRequestId)
            .NotEmpty().WithMessage("Return Request ID is required.");

        RuleFor(c => c.Status)
            .Must(status => status == ReturnStatus.Approved || status == ReturnStatus.Rejected)
            .WithMessage("Status must be either Approved or Rejected.");
    }
}
