using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Events.Models;

public class FluentResults : IFluentResults
{
    public FluentResultsStatus Status { get; set; }
    public List<string> Messages { get; set; } = new();
    public Dictionary<string, object> Keys { get; }

    public string ToMultiLine(string delimiter = null)
    {
        throw new NotImplementedException();
    }
}

public class FluentResults<T> : IFluentResults<T>
{
    public FluentResultsStatus Status { get; set; }
    public List<string> Messages { get; set; } = new();
    public Dictionary<string, object> Keys { get; }

    public string ToMultiLine(string delimiter = null)
    {
        throw new NotImplementedException();
    }

    public T Value { get; }
}
