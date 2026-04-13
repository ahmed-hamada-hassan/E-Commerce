using E_Commerce.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Common;

public class OffsetPaginationValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IOffsetPaginationRequest
{
    public OffsetPaginationValidator(IOptionsSnapshot<PaginationSettings> paginationSettings)
    {
        var maxSize = paginationSettings.Value.MaxSize;
        RuleFor(x => x.Size)
            .InclusiveBetween(1, maxSize)
            .WithMessage($"Page size must be between 1 and {maxSize}.");
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");
    }
}
