using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Finansik.Domain.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory)
{
    private readonly Meter _meter = meterFactory.Create("Finansik.Domain");
    private readonly ConcurrentDictionary<string, Counter<int>> _counters = new();
    internal static readonly ActivitySource ActivitySource = new("Finansik.Domain");

    public void IncrementCounter(string name, int delta, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = _counters.GetOrAdd(name, _ => _meter.CreateCounter<int>(name));
        counter.Add(delta, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public static IDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}