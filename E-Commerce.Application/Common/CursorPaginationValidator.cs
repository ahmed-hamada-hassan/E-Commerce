using E_Commerce.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Common;

public class CursorPaginationValidator<TRequest, TCursor> : AbstractValidator<TRequest> 
    where TRequest : ICursorPaginationRequest<TCursor>
    where TCursor : struct
{
    public CursorPaginationValidator(IOptionsSnapshot<PaginationSettings> paginationSettings)
    {
        var maxSize = paginationSettings.Value.MaxSize;

        RuleFor(x => x.Size)
            .InclusiveBetween(1, maxSize)
            .WithMessage($"Page size must be between 1 and {maxSize}.");
    }
}