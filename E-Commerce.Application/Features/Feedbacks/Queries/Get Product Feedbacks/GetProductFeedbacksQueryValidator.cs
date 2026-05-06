using E_Commerce.Application.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Feedbacks.Queries.Get_Product_Feedbacks;

internal sealed class GetProductFeedbacksQueryValidator : CursorPaginationValidator<GetProductFeedbacksQuery, Guid>
{
    public GetProductFeedbacksQueryValidator(IOptionsSnapshot<PaginationSettings> options): base(options)
    {
        RuleFor(q => q.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}
