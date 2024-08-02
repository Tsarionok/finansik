using MediatR;
using Microsoft.Extensions.Logging;

namespace Finansik.Domain.Monitoring;

internal class MonitoringPipelineBehavior<TRequest, TResponse>(
    DomainMetrics metrics,
    ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();

        try
        {
            var result = await next.Invoke();
            monitoredRequest.MonitorSuccess(metrics);
            return result;
        }
        catch (Exception exception)
        {
            monitoredRequest.MonitorFailure(metrics);
            logger.LogError(exception, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}