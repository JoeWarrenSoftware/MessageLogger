namespace MessageLogger.Contracts.Responses;
public class LogResponse
{
    public required Guid Id { get; init; }

    public required DateTime Date { get; init; }

    public required string Text { get; init; }
}
