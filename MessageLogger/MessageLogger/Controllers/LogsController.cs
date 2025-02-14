using MessageLogger.Application.Repositories;
using MessageLogger.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using MessageLogger.Api.Mapping;

namespace MessageLogger.Api.Controllers;

[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;

    public LogsController(ILogRepository logRepository)
    {
        _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
    }

    [HttpPost(ApiEndpoints.Logs.Create)]
    public async Task<IActionResult> Create([FromBody] CreateLogRequest request)
    {
        if (request.Text.Length > 255)
        {
            return BadRequest("Text cannot exceed 255 characters.");
        }

        var log = request.MapToLog();

        await _logRepository.CreateAsync(log);

        var response = log.MapToResponse();

        return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
    }

    [HttpGet(ApiEndpoints.Logs.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var log = await _logRepository.GetByIdAsync(id);

        if (log == null)
        {
            return NotFound();
        }

        var response = log.MapToResponse();

        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Logs.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var logs = await _logRepository.GetAllAsync();

        var logsResponse = logs.MapToResponse();

        return Ok(logsResponse);
    }
}