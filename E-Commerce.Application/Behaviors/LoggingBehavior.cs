using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace E_Commerce.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[START] Processing request {RequestName}", requestName);
        var timer = Stopwatch.StartNew();
        var response = await next();
        timer.Stop();
        _logger.LogInformation("[END] Processed request {RequestName} in {ElapsedMilliseconds} ms", requestName, timer.ElapsedMilliseconds);
        return response;
    }
}
