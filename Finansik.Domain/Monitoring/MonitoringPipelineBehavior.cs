using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Context.Propagation;

namespace Finansik.Domain.Monitoring;

internal abstract class MonitoringPipelineBehavior
{
    protected static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator; 
}

internal class MonitoringPipelineBehavior<TRequest, TResponse>(
    DomainMetrics metrics,
    ILogger<MonitoringPipelineBehavior<TRequest, TResponse>> logger) 
    : MonitoringPipelineBehavior, IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoredRequest monitoredRequest) return await next.Invoke();

        using var activity = DomainMetrics.ActivitySource.StartActivity(
            "usecase", ActivityKind.Internal, default(ActivityContext));
        activity?.AddTag("finansik.command", request.GetType().Name);

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