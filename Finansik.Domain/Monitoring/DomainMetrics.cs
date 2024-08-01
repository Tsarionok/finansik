using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace Finansik.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("Finansik.Domain");
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();

    public void GroupsFetched(bool success) =>
        IncrementCounter("groups_fetched", 1, new Dictionary<string, object?>
        {
            ["success"] = success
        });

    private void IncrementCounter(string name, int delta, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = _counters.GetOrAdd(name, _ => _meter.CreateCounter<int>(name));
        counter.Add(delta, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }
}