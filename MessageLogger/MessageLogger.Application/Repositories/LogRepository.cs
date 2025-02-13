using System.Text.Json;
using MessageLogger.Application.Models;

namespace MessageLogger.Application.Repositories;
public class LogRepository : ILogRepository
{
    private readonly string _filePath;

    public LogRepository(string filePath = "MessageLogs.txt")
    {
        _filePath = filePath;
    }

    public async Task<bool> CreateAsync(LogMessage logMessage)
    {
        var logEntry = JsonSerializer.Serialize(logMessage);
        await File.AppendAllTextAsync(_filePath, logEntry + Environment.NewLine);
        return true;
    }

    public async Task<LogMessage?> GetByIdAsync(Guid id)
    {
        var logs = await ReadAllLogsAsync();
        return logs.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<LogMessage>> GetAllAsync()
    {
        return await ReadAllLogsAsync();
    }

    private async Task<IEnumerable<LogMessage>> ReadAllLogsAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<LogMessage>();
        }

        var lines = await File.ReadAllLinesAsync(_filePath);
        var logs = new List<LogMessage>();

        foreach (var line in lines)
        {
            try
            {
                var log = JsonSerializer.Deserialize<LogMessage>(line);
                if (log != null)
                {
                    logs.Add(log);
                }
            }
            catch (JsonException)
            {
                // Ignore bad-parsed logs
            }
        }

        return logs;
    }

}
