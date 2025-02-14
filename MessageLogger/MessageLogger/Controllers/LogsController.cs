using MessageLogger.Application.Repositories;
using MessageLogger.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using MessageLogger.Api.Mapping;
using Microsoft.Extensions.Logging;

namespace MessageLogger.Api.Controllers;

[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;
    private readonly ILogger<LogsController> _logger;

    public LogsController(ILogRepository logRepository, ILogger<LogsController> logger)
    {
        _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost(ApiEndpoints.Logs.Create)]
    public async Task<IActionResult> Create([FromBody] CreateLogRequest request)
    {
        try
        {
            if (request.Text.Length > 255)
            {
                return BadRequest("Text cannot exceed 255 characters.");
            }

            var log = request.MapToLog();

            await _logRepository.CreateAsync(log);

            var response = log.MapToResponse();

            _logger.LogInformation("Log created successfully: {LogId}", log.Id);

            return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a log entry");
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    [HttpGet(ApiEndpoints.Logs.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        try
        {
            var log = await _logRepository.GetByIdAsync(id);

            if (log == null)
            {
                _logger.LogWarning("Log not found: {LogId}", id);
                return NotFound();
            }

            var response = log.MapToResponse();

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting a single log entry");
            return StatusCode(500, "An internal server error occurred.");
        }
    }

    [HttpGet(ApiEndpoints.Logs.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var logs = await _logRepository.GetAllAsync();

            var logsResponse = logs.MapToResponse();

            return Ok(logsResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all log entries");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}