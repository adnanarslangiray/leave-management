using MediatR;
using Microsoft.Extensions.Logging;

namespace LeaveManagement.Application.PipelineBehaviours;

public class ExceptionBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public ExceptionBehaviours(ILogger<TRequest> logger)
    {
        _logger=logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception)
        {
            var name = typeof(TRequest).Name;
            _logger.LogError("Application Request: Unhandled Exception for Request {Name} {@Request}", name, request);
            throw;
        }
    }
}