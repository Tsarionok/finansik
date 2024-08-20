using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Context.Propagation;

namespace Finansik.Domain.Monitoring;

internal abstract class MonitoringPipelineBehavior
{
    protected static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator; 
}

internal sealed class MonitoringPipelineBehavior<TRequest, TResponse>(
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
            activity?.AddTag("error", false);
            logger.LogInformation("Command successfully handled {Command}", request);
            return result;
        }
        catch (Exception exception)
        {
            monitoredRequest.MonitorFailure(metrics);
            activity?.AddTag("error", true);
            logger.LogError(exception, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}