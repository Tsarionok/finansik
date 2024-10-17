namespace Finansik.Domain.Monitoring;

public abstract record DefaultMonitoredRequest(string CounterName) : IMonitoredRequest
{
    public void MonitorSuccess(DomainMetrics metrics) =>
        metrics.IncrementCounter(CounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) =>
        metrics.IncrementCounter(CounterName, 1, DomainMetrics.ResultTags(false));
}