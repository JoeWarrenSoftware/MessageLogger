using MessageLogger.Application.Models;
using MessageLogger.Contracts.Requests;
using MessageLogger.Contracts.Responses;

namespace MessageLogger.Api.Mapping;

public static class ContractMapping
{
    public static LogMessage MapToLog(this CreateLogRequest request)
    {
        return new LogMessage
        {
            Id = Guid.CreateVersion7(),
            Date = DateTime.UtcNow,
            Text = request.Text,
        };
    }

    public static LogResponse MapToResponse(this LogMessage log)
    {
        return new LogResponse
        {
            Id = log.Id,
            Date = log.Date,
            Text = log.Text,
        };
    }

    public static LogsResponse MapToResponse(this IEnumerable<LogMessage> logs)
    {
        return new LogsResponse
        {
            Items = logs.Select(MapToResponse)
        };
    }
}
