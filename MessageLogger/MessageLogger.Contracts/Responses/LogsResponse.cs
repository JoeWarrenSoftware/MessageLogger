namespace MessageLogger.Contracts.Responses;
public class LogsResponse
{
    public required IEnumerable<LogResponse> Items { get; init; } = Enumerable.Empty<LogResponse>();
}