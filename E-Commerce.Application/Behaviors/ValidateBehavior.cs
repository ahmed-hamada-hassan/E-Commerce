using FluentValidation;
using MediatR;

namespace E_Commerce.Application.Behaviors;

public sealed class ValidateBehavior<TRequst, TResponse> : IPipelineBehavior<TRequst, TResponse> where TRequst : notnull
{
    private readonly IEnumerable<IValidator<TRequst>> _validators;

    public ValidateBehavior(IEnumerable<IValidator<TRequst>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequst request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequst>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}
